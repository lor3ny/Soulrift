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
        PrintMatrix();
        
    }


    private void ProceduralGenerator_2()
    {

        int k = 10;

        // Default
        matrix[center, center] = 1;
        matrix[center, center+1] = 1;

        // Posizionare gli 1
        for(int i = 1; i < matSize-1; i++)
        {
            for (int j = 1; j < matSize-1; j++)
            {
                // Verifica se c'è una stanza nei riquadri vicini, se così fosse valuta se piazzare uno
                // Se non ci sono stanze ma solo quella di default, vale solo sopra e deve essere piazzata al cento per cento
                // Mettere tappi (markati con 2:Left, 3:Up, 4:Right, 5:Down), in uno di questi mettere ending point (markato con 6)

                // Mettere effettivamente le stanza, seguendo accuratamente le posizioni nello spazio (capire quando sono grandi le stanze)


                if(matrix[i + 1, j] == 1 || matrix[i - 1, j] == 1 || matrix[i, j + 1] == 1 || matrix[i, j - 1] == 1)
                {
                    Debug.Log("MAYBE");
                    bool goOn = FlipCoin();
                    if (!goOn)
                    {
                        continue;
                    }
                }
            }
        }

        // Posizionare tappi

        // Posizionare ending
    }


    private void PrintMatrix()
    {
        for (int i = 0; i < matSize; i++)
        {
            Debug.Log(i);
            string matText = "[";
            for (int j = 0; j < matSize; j++)
            {
                matText += matrix[i, j].ToString()+", ";
            }
            matText += "]";
            Debug.Log(matText);
        }
    }

    private void ProceduralGenerate()
    {
        List<Room> roomsListAus = new List<Room>(roomsList);
        List<Room> activatedRooms = new List<Room>();
        int x;
        int y;

        while (roomsListAus.Count > 0) 
        {
            Room pointedRoom = roomsSelected[0];
            bool oneRoom = false;
            x = pointedRoom.x;
            y = pointedRoom.y;

            if (matrix[x + 1, y] == 1 && matrix[x - 1, y] == 1 && matrix[x, y + 1] == 1 && matrix[x, y - 1] == 1)
            {
                roomsSelected.RemoveAt(0);
                continue;
            }

            Debug.Log("(" + x.ToString() + ", " + y.ToString() + ")");
            Debug.Log(chanceOfOne);

            Shuffle<Location>(pointedRoom.locations);

            for (int i = 0; i < pointedRoom.locations.Count; ++i)
            {

                // Randomly decide if there is going to be a room
                if (chanceOfOne <= 0f)
                {
                    Debug.Log("Map done! chances over.");
                    return;
                }

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
                newRoom.gameObject.SetActive(true);
                newRoom.GetComponent<Room>().Setup();
                newRoom.transform.position = Vector3.zero;

                switch (pointedRoom.locations[i])
                {
                    case Location.UP:
                        if (matrix[x, y+1] == 1)
                            continue;

                        newRoom.gameObject.transform.position = pointedRoom.UP.transform.position /*+ pointedRoom.gameObject.transform.position*/ - newRoom.DOWN.transform.position;
                        newRoom.locations.Remove(Location.DOWN);
                        pointedRoom.locations.Remove(Location.UP);
                        matrix[x, y + 1] = 1;
                        newRoom.x = x;
                        newRoom.y = y + 1;
                        break;

                    case Location.DOWN:
                        if (matrix[x, y-1] == 1)
                            continue;

                        newRoom.gameObject.transform.position = pointedRoom.DOWN.transform.position /*+ pointedRoom.gameObject.transform.position*/ - newRoom.UP.transform.position;
                        newRoom.locations.Remove(Location.UP);
                        pointedRoom.locations.Remove(Location.DOWN);
                        matrix[x, y - 1] = 1;
                        newRoom.x = x;
                        newRoom.y = y - 1;
                        break;

                    case Location.LEFT:
                        if (matrix[x-1, y] == 1)
                            continue;

                        newRoom.gameObject.transform.position = pointedRoom.LEFT.transform.position /*+ pointedRoom.gameObject.transform.position*/ - newRoom.RIGHT.transform.position;
                        newRoom.locations.Remove(Location.RIGHT);
                        pointedRoom.locations.Remove(Location.LEFT);
                        matrix[x-1, y] = 1;
                        newRoom.x = x-1;
                        newRoom.y = y;
                        break;

                    case Location.RIGHT:
                        if (matrix[x + 1, y] == 1)
                            continue;

                        newRoom.gameObject.transform.position = pointedRoom.RIGHT.transform.position /*+ pointedRoom.gameObject.transform.position*/ - newRoom.LEFT.transform.position;
                        newRoom.locations.Remove(Location.LEFT);
                        pointedRoom.locations.Remove(Location.RIGHT);
                        matrix[x+1, y] = 1;
                        newRoom.x = x+1;
                        newRoom.y = y;
                        break;

                    default:
                        Debug.LogError("Problems with location!");
                        break;
                }

                roomsListAus.RemoveAt(selectedRoom);
                roomsSelected.RemoveAt(0);
                roomsSelected.Add(newRoom);
                activatedRooms.Add(newRoom);
            }
        }


        List<GameObject> cups = new List<GameObject>();
        bool isLast = false;
        bool endingDone = false;

        int endingCount = 0;
        foreach (Room room in activatedRooms)
        {
            if(endingCount > roomsList.Count/2 && !endingDone)
            {
                isLast = true;
            }

            foreach (Location location in room.locations)
            {
                GameObject ending;
                switch (location)
                {
                    case Location.UP:
                        if (matrix[room.x, room.y+1] == 1)
                            continue;

                        if (isLast)
                        {
                            isLast = false;
                            endingDone = true;
                            endingRoomUP.SetActive(true);
                            endingRoomUP.transform.position = room.UP.transform.position - endingRoomUP.GetComponent<Room>().DOWN.transform.position;
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

                        if (isLast)
                        {
                            isLast = false;
                            endingDone = true;
                            endingRoomDOWN.SetActive(true);
                            endingRoomDOWN.transform.position = room.DOWN.transform.position - endingRoomDOWN.GetComponent<Room>().UP.transform.position;
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


                        if (isLast)
                        {
                            isLast = false;
                            endingDone = true;
                            endingRoomLEFT.SetActive(true);
                            endingRoomLEFT.transform.position = room.LEFT.transform.position - endingRoomLEFT.GetComponent<Room>().RIGHT.transform.position;
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

                        if (isLast)
                        {
                            isLast = false;
                            endingDone = true;
                            endingRoomRIGHT.SetActive(true);
                            endingRoomRIGHT.transform.position = room.RIGHT.transform.position - endingRoomRIGHT.GetComponent<Room>().LEFT.transform.position;
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
            endingCount++;
        }

        /*
        // Bisogna farlo con la matrice
        // Piazza la ending room bene, come fare?
        Debug.Log(roomsList.Count);

        Room lastRoom = roomsList[roomsList.Count - 1];
        endingRoomUP.SetActive(true);
        endingRoomUP.transform.position = lastRoom.UP.transform.position - endingRoomUP.GetComponent<Room>().DOWN.transform.position;
        */

        // PUT PLUGS IN ENDING ROOMS

        Debug.Log("Map done! rooms over.");
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
