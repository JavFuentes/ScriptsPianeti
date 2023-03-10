using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{   
    //Color usado cuando el planeta es seleccionado
    private static Color selectedColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);

    //Referencia al planeta previamente seleccionado
    private static Planet previousSelected = null;

    //Referencia al SpriteRenderer del planeta
    private SpriteRenderer spriteRenderer;

     //Indica si el planeta está seleccionado
    private bool isSelected = false;

    //Referencia al Collider2D del planeta
    private new Collider2D collider;

    //Id del planeta
    public int id;

    //Direcciones adyacentes del planeta
    private Vector2[] adjacentDirections = new Vector2[]
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };

    private void Awake()
    {   
        //Obtiene la referencia al SpriteRenderer del planeta
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {   
        //Obtiene la referencia al Collider2D del planeta
        collider = GetComponent<Collider2D>();
    }

    //Selecciona el planeta
    private void SelectPlanet()
    {
        isSelected = true;
        spriteRenderer.color = selectedColor;
        previousSelected = gameObject.GetComponent<Planet>();
    }

    //Deselecciona el planeta
    private void DeselectedPlanet()
    {
        isSelected = false;
        spriteRenderer.color = Color.white;
        previousSelected = null;
    }

    //Detecta el touch en el planeta
    bool CheckForTouch()
    {   
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {   
            //Obtiene la posición del toque
            var wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            var touchPosition = new Vector2(wp.x, wp.y);

            //Comprueba si el toque se realizó sobre el Collider2D del planeta
            if (collider == Physics2D.OverlapPoint(touchPosition))
            {
              OnTouchDown();
            }            
        }
        return false;
    }

    void Update()
    {        
        CheckForTouch();     
    }

    //Función que se llama al tocar el planeta
    private void OnTouchDown()
    {   
        
        if (spriteRenderer.sprite == null || BoardManager.sharedInstance.isShifting)
        {   
            //Si el SpriteRenderer del planeta es nulo o el tablero está cambiando, no hace nada
            return;
        }

        if (isSelected)
        {   
            //Si el planeta ya está seleccionado, lo deselecciona
            DeselectedPlanet();
        }
        else
        {
            if (previousSelected == null)
            {   
                //Si no hay ningún planeta previamente seleccionado, selecciona este
                SelectPlanet();
            }
            else
            {
                if (CanSwipe())
                {   
                    //Si se puede hacer un intercambio con el planeta previamente seleccionado, realiza el intercambio y busca coincidencias
                    SwapSprite(previousSelected);
                    previousSelected.FindAllMatches();
                    previousSelected.DeselectedPlanet();
                    FindAllMatches();

                    //Reduce el contador de movimientos
                    GUIManager.sharedInstance.MoveCounter--;
                }
                else
                {   
                    //Si no se puede hacer un intercambio, deselecciona el planeta
                    previousSelected.DeselectedPlanet();
                    SelectPlanet(); 
                }
            }
        }        
    }

    public void SwapSprite(Planet newPlanet)
    {   
        // Comprueba si la imagen del planeta actual y del nuevo planeta son iguales
        if (spriteRenderer.sprite == newPlanet.GetComponent<SpriteRenderer>().sprite)
        {   
            // Si son iguales, no hace nada
            return;
        }

        // Intercambia las imágenes de los planetas y sus IDs
        Sprite oldPlanet = newPlanet.spriteRenderer.sprite;
        newPlanet.spriteRenderer.sprite = this.spriteRenderer.sprite;
        this.spriteRenderer.sprite = oldPlanet;

        int tempId = newPlanet.id;
        newPlanet.id = this.id;
        this.id = tempId;
    }

    private GameObject GetNeighbor(Vector2 direccion)
    {   
        // Hace un raycast en la dirección especificada para encontrar el vecino del planeta actual
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direccion);
        if (hit.collider != null)
        {   
            // Si se encuentra un vecino, devuelve su objeto
            return hit.collider.gameObject;
        }
        else
        {   
            // Si no se encuentra un vecino, devuelve null
            return null;
        }
    }

    private List<GameObject> GetAllNeighbors()
    {
        List<GameObject> neighbors = new List<GameObject>();

        // Itera sobre las direcciones adyacentes para encontrar todos los vecinos del planeta actual
        foreach (Vector2 direction in adjacentDirections)
        {
            neighbors.Add(GetNeighbor(direction));
        }
        return neighbors;
    }

    private bool CanSwipe()
    {   
        // Comprueba si el planeta anteriormente seleccionado está entre los vecinos del planeta actual
        return GetAllNeighbors().Contains(previousSelected.gameObject);
    }

    private List<GameObject> FindMatch(Vector2 direction)
    {
        List<GameObject> matchingPlanets = new List<GameObject>();
        
        // Hace un raycast en la dirección especificada para encontrar planetas con la misma imagen que el planeta actual
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction);

        while (hit.collider != null && hit.collider.GetComponent<SpriteRenderer>().sprite == spriteRenderer.sprite)
        {
            matchingPlanets.Add(hit.collider.gameObject);
            hit = Physics2D.Raycast(hit.collider.transform.position, direction);
        }
        
        return matchingPlanets;
    }

    private bool ClearMatch(Vector2[] directions)
    {
        List<GameObject> matchingPlanets = new List<GameObject>();

        // Encuentra todos los planetas que tienen coincidencias en las direcciones especificadas
        foreach (Vector2 direction in directions)
        {
            matchingPlanets.AddRange(FindMatch(direction));
        }

        if (matchingPlanets.Count >= BoardManager.MinPlanetsToMatch)
        {   
            //Si se hacen 3 en linea 
            if(matchingPlanets.Count == 2)
            {
                GUIManager.sharedInstance.Score += 10;          
            }

            //Si se hacen 4 en linea obtienes 1 movimiento      
            if(matchingPlanets.Count == 3)
            {
                GUIManager.sharedInstance.MoveCounter += 1; 
                GUIManager.sharedInstance.Score += 10;  
            }

            //Si se hacen 5 en linea ganas 3 movimientos
            else if(matchingPlanets.Count == 4)
            {
                GUIManager.sharedInstance.MoveCounter += 3;
                GUIManager.sharedInstance.Score += 50;                   
            }

            // Elimina las imágenes de los planetas que coinciden
            foreach (GameObject planet in matchingPlanets)
            {
                planet.GetComponent<SpriteRenderer>().sprite = null;                                                   
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public void FindAllMatches()
    {
        if (spriteRenderer.sprite == null)
        {
            return;
        }

        // Busca coincidencias en las direcciones horizontal y vertical
        bool hMatch = ClearMatch(new Vector2[2]
        {
            Vector2.left, Vector2.right });

        bool vMatch = ClearMatch(new Vector2[2]
        {
            Vector2.up, Vector2.down });

        if (hMatch || vMatch)
        {
            spriteRenderer.sprite = null;
            
            StopCoroutine(BoardManager.sharedInstance.FindNullPlanets());
            StartCoroutine(BoardManager.sharedInstance.FindNullPlanets());
        }        
    }
}
