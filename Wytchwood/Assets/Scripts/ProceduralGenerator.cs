using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Room;
using static UnityEngine.Rendering.DebugUI;

public class ProceduralGenerator : MonoBehaviour
{


    public Room startingRoom;
    public List<Room> roomsList;

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
        matrix[center, center] = 1;

        roomsSelected.Add(startingRoom);
        ProceduralGenerate(); // Forse va cambiato
    }


    private void ProceduralGenerator_2()
    {
        for(int i = 1; i < matSize-1; i++)
        {
            for (int j = 1; j < matSize-1; j++)
            {


                // Verifica se c'è una stanza nei riquadri vicini, se così fosse valuta se piazzare uno
                // Se non ci sono stanze ma solo quella di default, vale solo sopra e deve essere piazzata al cento per cento
                // Mettere tappi (markati con 2:Left, 3:Up, 4:Right, 5:Down), in uno di questi mettere ending point (markato con 6)

                // Mettere effettivamente le stanza, seguendo accuratamente le posizioni nello spazio (capire quando sono grandi le stanze)


                matrix[i+1, j] = 1;
                matrix[i-1, j] = 1;
                matrix[i, j+1] = 1;
                matrix[i, j-1] = 1;
            }
        }
    }


    private void ProceduralGenerate()
    {
        List<Room> roomsListAus = new List<Room>(roomsList);
        List<Room> activatedRooms = new List<Room>();

        while (roomsListAus.Count > 0) 
        {
            Room pointedRoom = roomsSelected[0];
            bool oneRoom = false;
            Debug.Log(chanceOfOne);

            List<Location> locations = pointedRoom.GetLocations();
            Shuffle<Location>(locations);

            for (int i = 0; i < locations.Count; ++i)
            {

                // Randomly decide if there is going to be a room
                if (chanceOfOne <= 0f)
                {
                    Debug.Log("Map done! chances over.");
                    return;
                }


                Debug.Log(pointedRoom.name+" Door: " + locations[i]);

                bool goOn = false;
                if(i == locations.Count-1 && oneRoom == false)
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

                int selectedRoom = FindTheRoom(locations[i], roomsListAus.Count);
                if (selectedRoom == -1)
                    continue;
                Debug.Log("Selected: " + roomsListAus[selectedRoom].name);
                oneRoom = true;

                // Spawn
                Room newRoom = roomsListAus[selectedRoom];
                newRoom.gameObject.SetActive(true);
                newRoom.GetComponent<Room>().Setup();
                newRoom.transform.position = Vector3.zero;
 
                switch (locations[i])
                {
                    case Location.UP:
                        newRoom.gameObject.transform.position = pointedRoom.UP.transform.position /*+ pointedRoom.gameObject.transform.position*/ - newRoom.DOWN.transform.position;
                        newRoom.GetLocations().Remove(Location.DOWN);
                        pointedRoom.GetLocations().Remove(Location.UP);
                        break;

                    case Location.DOWN:
                        newRoom.gameObject.transform.position = pointedRoom.DOWN.transform.position /*+ pointedRoom.gameObject.transform.position*/ - newRoom.UP.transform.position;
                        newRoom.GetLocations().Remove(Location.UP);
                        pointedRoom.GetLocations().Remove(Location.DOWN);
                        break;

                    case Location.LEFT:
                        newRoom.gameObject.transform.position = pointedRoom.LEFT.transform.position /*+ pointedRoom.gameObject.transform.position*/ - newRoom.RIGHT.transform.position;
                        newRoom.GetLocations().Remove(Location.RIGHT);
                        pointedRoom.GetLocations().Remove(Location.LEFT);
                        break;

                    case Location.RIGHT:
                        newRoom.gameObject.transform.position = pointedRoom.RIGHT.transform.position /*+ pointedRoom.gameObject.transform.position*/ - newRoom.LEFT.transform.position;
                        newRoom.GetLocations().Remove(Location.LEFT);
                        pointedRoom.GetLocations().Remove(Location.RIGHT);
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

        // Bisogna farlo con la matrice
        // Piazza la ending room bene, come fare?
        Debug.Log(roomsList.Count);

        Room lastRoom = roomsList[roomsList.Count - 1];
        endingRoomUP.SetActive(true);
        endingRoomUP.transform.position = lastRoom.UP.transform.position - endingRoomUP.GetComponent<Room>().DOWN.transform.position;

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
