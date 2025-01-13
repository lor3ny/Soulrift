using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Room;
using static Unity.Burst.Intrinsics.X86.Avx;
using static UnityEngine.Rendering.DebugUI;

public class ProceduralGenerator : MonoBehaviour
{


    public Room startingRoom;
    public List<Room> roomsList;

    public GameObject mainGrid;

    public GameObject plugUP;
    public GameObject plugDOWN;
    public GameObject plugRIGHT;
    public GameObject plugLEFT;

    public GameObject endingRoomUP;
    public GameObject endingRoomDOWN;
    public GameObject endingRoomRIGHT;
    public GameObject endingRoomLEFT;

    private List<Room> roomsSelected = new List<Room>();
    private float chanceOfOne = 50f;
    private float decreaseAmount = 0f;  // Probabilmente non serve

    byte[,] matrix;
    int matSize;
    int center;


    // Bisogna inserire un controllo per evitare che si sovrappongano delle rooms in livelli molto grandi
    // Nel caso di stanza tutte grandi max un tot si può usare una matrice
    // Per stanze con grandezze diverse bisogna valutare l'occupancy in realtime, tramite un collision detection o altro
    // Bisogna mettere il "tappo" alle porte che non hanno connessione (creare delle porte tappo prefab e le instanzi all'occorrenza)

    public void StartGenerate()
    {
        matSize = roomsList.Count * 2 + 1;
        matrix = new byte[matSize, matSize];
        center = roomsList.Count;
 
        startingRoom.gameObject.SetActive(true);
        startingRoom.gameObject.transform.position = Vector2.zero;
        startingRoom.x = center;
        startingRoom.y = center;
        matrix[center, center] = 1;

        roomsSelected.Add(startingRoom);
        ProceduralGenerate(); // Forse va cambiato
        
    }


    private void ProceduralGenerate()
    {
        List<Room> roomsListAus = new List<Room>(roomsList);
        List<Room> activatedRooms = new List<Room>();
        int x;
        int y;

        while (roomsListAus.Count > 0) 
        {
            if (roomsSelected.Count == 0)
            {
                break;
            }

            Room pointedRoom = roomsSelected[0];
            bool oneRoom = false;
            x = pointedRoom.x;
            y = pointedRoom.y;

            if (matrix[x + 1, y] == 1 && matrix[x - 1, y] == 1 && matrix[x, y + 1] == 1 && matrix[x, y - 1] == 1)
            {
                roomsSelected.RemoveAt(0);
                continue;
            }

            matrix[pointedRoom.x, pointedRoom.y] = 1;

            Debug.Log("(" + x.ToString() + ", " + y.ToString() + ")");

            Shuffle<Location>(pointedRoom.locations);

            for (int i = 0; i < pointedRoom.locations.Count; ++i)
            {

                Debug.Log(pointedRoom.name+" Door: " + pointedRoom.locations[i]);

                bool goOn = false;
                if(i == pointedRoom.locations.Count-1 && oneRoom == false)
                {
                    goOn = true;
                } else
                {
                    goOn = FlipCoin();
                }
                if (!goOn)
                {
                    continue;
                }

                int selectedRoom = FindTheRoom(pointedRoom.locations[i], roomsListAus.Count);
                if (selectedRoom == -1)
                    continue;

                Debug.Log("Selected: " + roomsListAus[selectedRoom].name);
                oneRoom = true;

                // Spawn
                Room newRoom = roomsListAus[selectedRoom];

                bool isPlaced = false;

                switch (pointedRoom.locations[i])
                {
                    case Location.UP:
                        Debug.Log("(" + x.ToString() + "," + (y + 1).ToString() + ")");
                        Debug.Log(matrix[x, y + 1]);
                        if (matrix[x, y+1] == 1)
                            break;

                        Debug.Log("Passed");
                        newRoom.gameObject.SetActive(true);
                        newRoom.GetComponent<Room>().Setup();
                        newRoom.transform.position = Vector3.zero;
                        newRoom.gameObject.transform.position = pointedRoom.UP.transform.position /*+ pointedRoom.gameObject.transform.position*/ - newRoom.DOWN.transform.position;
                        newRoom.locations.Remove(Location.DOWN);
                        pointedRoom.locations.Remove(Location.UP);
                        //matrix[x, y + 1] = 1;
                        newRoom.x = x;
                        newRoom.y = y + 1;
                        isPlaced = true;
                        break;

                    case Location.DOWN:
                        Debug.Log("(" + x.ToString() + "," + (y-1).ToString() + ")");
                        Debug.Log(matrix[x, y-1]);
                        if (matrix[x, y-1] == 1)
                            break;

                        Debug.Log("Passed");
                        newRoom.gameObject.SetActive(true);
                        newRoom.GetComponent<Room>().Setup();
                        newRoom.transform.position = Vector3.zero;
                        newRoom.gameObject.transform.position = pointedRoom.DOWN.transform.position /*+ pointedRoom.gameObject.transform.position*/ - newRoom.UP.transform.position;
                        newRoom.locations.Remove(Location.UP);
                        pointedRoom.locations.Remove(Location.DOWN);
                        //matrix[x, y - 1] = 1;
                        newRoom.x = x;
                        newRoom.y = y - 1;
                        isPlaced = true;
                        break;

                    case Location.LEFT:
                        Debug.Log("(" + (x - 1).ToString() + "," + y.ToString() + ")");
                        Debug.Log(matrix[x - 1, y]);
                        if (matrix[x-1, y] == 1)
                            break;

                        Debug.Log("Passed");
                        newRoom.gameObject.SetActive(true);
                        newRoom.GetComponent<Room>().Setup();
                        newRoom.transform.position = Vector3.zero;
                        newRoom.gameObject.transform.position = pointedRoom.LEFT.transform.position /*+ pointedRoom.gameObject.transform.position*/ - newRoom.RIGHT.transform.position;
                        newRoom.locations.Remove(Location.RIGHT);
                        pointedRoom.locations.Remove(Location.LEFT);
                        matrix[x-1, y] = 1;
                        newRoom.x = x-1;
                        newRoom.y = y;
                        isPlaced = true;
                        break;

                    case Location.RIGHT:
                        Debug.Log("(" + (x+1).ToString() + "," + y.ToString() + ")");
                        Debug.Log(matrix[x + 1, y]);
                        if (matrix[x + 1, y] == 1)
                            break;

                        newRoom.gameObject.SetActive(true);
                        newRoom.GetComponent<Room>().Setup();
                        newRoom.transform.position = Vector3.zero;
                        newRoom.gameObject.transform.position = pointedRoom.RIGHT.transform.position /*+ pointedRoom.gameObject.transform.position*/ - newRoom.LEFT.transform.position;
                        newRoom.locations.Remove(Location.LEFT);
                        pointedRoom.locations.Remove(Location.RIGHT);
                        //matrix[x+1, y] = 1;
                        newRoom.x = x+1;
                        newRoom.y = y;
                        isPlaced = true;
                        break;

                    default:
                        Debug.LogError("Problems with location!");
                        break;
                }

                if (isPlaced)
                {
                    roomsListAus.RemoveAt(selectedRoom);
                    roomsSelected.Add(newRoom);
                    activatedRooms.Add(newRoom);
                }
            }

            roomsSelected.RemoveAt(0);
        }


        List<GameObject> cups = new List<GameObject>();

        bool done = false;   
        for(int i = activatedRooms.Count-1; i >= 0; i--)
        {
            Room room = activatedRooms[i];
            foreach (Location location in room.locations)
            {
                GameObject ending;
                switch (location)
                {
                    case Location.UP:
                        if (matrix[room.x, room.y+1] == 1)
                            break;

                        if (!done)
                        {
                            Debug.Log(room.name + " Door: " + location);
                            endingRoomUP.SetActive(true);
                            endingRoomUP.transform.position = room.UP.transform.position /*+ pointedRoom.gameObject.transform.position*/ - endingRoomUP.GetComponent<Room>().UP.transform.position;
                            done = true;
                            break;
                        }

                        ending = Instantiate(plugUP);
                        ending.transform.position = room.UP.transform.position /*+ pointedRoom.gameObject.transform.position*/ - ending.GetComponent<Room>().DOWN.transform.position;
                        ending.transform.SetParent(mainGrid.transform);
                        cups.Add(ending);
                        break;

                    case Location.DOWN:
                        if (matrix[room.x, room.y - 1] == 1)
                            break;

                        if (!done)
                        {
                            Debug.Log(room.name + " Door: " + location);
                            endingRoomDOWN.SetActive(true);
                            endingRoomDOWN.transform.position = room.DOWN.transform.position /*+ pointedRoom.gameObject.transform.position*/ - endingRoomDOWN.GetComponent<Room>().UP.transform.position;
                            done = true;
                            break;
                        }

                        ending = Instantiate(plugDOWN);
                        ending.transform.position = room.DOWN.transform.position /*+ pointedRoom.gameObject.transform.position*/ - ending.GetComponent<Room>().UP.transform.position;
                        ending.transform.SetParent(mainGrid.transform);
                        cups.Add(ending);
                        break;

                    case Location.LEFT:
                        if (matrix[room.x-1, room.y] == 1)
                            break;

                        if (!done)
                        {
                            Debug.Log(room.name + " Door: " + location);
                            endingRoomLEFT.SetActive(true);
                            endingRoomLEFT.transform.position = room.LEFT.transform.position /*+ pointedRoom.gameObject.transform.position*/ - endingRoomLEFT.GetComponent<Room>().UP.transform.position;
                            done = true;
                            break;
                        }

                        ending = Instantiate(plugLEFT);
                        ending.transform.position = room.LEFT.transform.position /*+ pointedRoom.gameObject.transform.position*/ - ending.GetComponent<Room>().RIGHT.transform.position;
                        ending.transform.SetParent(mainGrid.transform);
                        cups.Add(ending);
                        break;

                    case Location.RIGHT:
                        if (matrix[room.x+1, room.y] == 1)
                            break;

                        if (!done)
                        {
                            Debug.Log(room.name + " Door: " + location);
                            endingRoomRIGHT.SetActive(true);
                            endingRoomRIGHT.transform.position = room.RIGHT.transform.position /*+ pointedRoom.gameObject.transform.position*/ - endingRoomRIGHT.GetComponent<Room>().UP.transform.position;
                            done = true;
                            break;
                        }

                        ending = Instantiate(plugRIGHT);
                        ending.transform.position = room.RIGHT.transform.position /*+ pointedRoom.gameObject.transform.position*/ - ending.GetComponent<Room>().LEFT.transform.position;
                        ending.transform.SetParent(mainGrid.transform);
                        cups.Add(ending);
                        break;

                    default:
                        Debug.LogError("Problems with location!");
                        break;
                }
            }
        }
        
        return;
    }

    public void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);

            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private bool FlipCoin()
    {
        float randomValue = Random.Range(0f, 100f);

        bool result = randomValue < chanceOfOne ? true : false;

        chanceOfOne = Mathf.Max(0, chanceOfOne - decreaseAmount);

        return result;
    }

    private int FindTheRoom(Location loc, int roomsListCount)
    {

        List<int> possibleRooms = new List<int>();
        for (int i = 0; i < roomsListCount; ++i)
        {
            if (roomsList[i].GetLocations().Contains(loc))
            {
                possibleRooms.Add(i);
            }  
        }

        if(possibleRooms.Count == 0)
        {
            return -1;
        }

        // Pesca la prima disponibile, aggiungere randomicità
        return possibleRooms[0];
    }


}
