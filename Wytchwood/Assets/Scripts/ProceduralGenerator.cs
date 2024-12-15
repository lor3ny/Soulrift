using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Room;
using static UnityEngine.Rendering.DebugUI;

public class ProceduralGenerator : MonoBehaviour
{


    public Room startingRoom;
    public Room endingRoom;
    public List<Room> roomsList;
    
    private List<Room> roomsSelected = new List<Room>();
    private float chanceOfOne = 100f;
    private float decreaseAmount = 0f;  // Probabilmente non serve

    // Bisogna inserire un controllo per evitare che si sovrappongano delle rooms in livelli molto grandi
    // Nel caso di stanza tutte grandi max un tot si può usare una matrice
    // Per stanze con grandezze diverse bisogna valutare l'occupancy in realtime, tramite un collision detection o altro
    // Bisogna mettere il "tappo" alle porte che non hanno connessione (creare delle porte tappo prefab e le instanzi all'occorrenza)

    public void StartGenerate()
    {
        startingRoom.gameObject.SetActive(true);
        startingRoom.gameObject.transform.position = Vector2.zero;

        roomsSelected.Add(startingRoom);

        ProceduralGenerate();
    }

    private void ProceduralGenerate()
    {
        List<Room> roomsListAus = roomsList;
        List<Room> activatedRooms = new List<Room>();

        while (roomsSelected.Count > 0) 
        {
            Room pointedRoom = roomsSelected[0];
            roomsSelected.RemoveAt(0);
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

                bool goOn = FlipCoin();
                if (!goOn)
                {
                    // Mettere tappo
                    continue;
                }

                int selectedRoom = FindTheRoom(locations[i], roomsListAus.Count);
                if (selectedRoom == -1)
                    continue;
                Debug.Log("Selected: " + roomsListAus[selectedRoom].name);

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
                        //newRoom.DOWN.SetActive(false);
                        break;

                    case Location.DOWN:
                        newRoom.gameObject.transform.position = pointedRoom.DOWN.transform.position /*+ pointedRoom.gameObject.transform.position*/ - newRoom.UP.transform.position;
                        newRoom.GetLocations().Remove(Location.UP);
                        //newRoom.UP.SetActive(false);
                        break;

                    case Location.LEFT:
                        newRoom.gameObject.transform.position = pointedRoom.LEFT.transform.position /*+ pointedRoom.gameObject.transform.position*/ - newRoom.RIGHT.transform.position;
                        newRoom.GetLocations().Remove(Location.RIGHT);
                        //newRoom.RIGHT.SetActive(false);
                        break;

                    case Location.RIGHT:
                        newRoom.gameObject.transform.position = pointedRoom.RIGHT.transform.position /*+ pointedRoom.gameObject.transform.position*/ - newRoom.LEFT.transform.position;
                        newRoom.GetLocations().Remove(Location.LEFT);
                        //newRoom.LEFT.SetActive(false);
                        break;

                    default:
                        Debug.LogError("Problems with location!");
                        break;
                }

                roomsListAus.RemoveAt(selectedRoom);
                roomsSelected.Add(newRoom);
                activatedRooms.Add(newRoom);
            }
            Debug.Log(roomsSelected.Count);
            endingRoom.gameObject.SetActive(true);
            endingRoom.GetComponent<Room>().Setup();
            endingRoom.transform.position = Vector3.zero;
            endingRoom.gameObject.transform.position = activatedRooms[activatedRooms.Count - 1].UP.transform.position - endingRoom.DOWN.transform.position;
        }

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
