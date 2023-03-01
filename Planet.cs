using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    private static Color selectedColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
    private static Planet previousSelected = null;
    private SpriteRenderer spriteRenderer;
    private bool isSelected = false;
    private Collider2D collider;

    public int id;

    private Vector2[] adjacentDirections = new Vector2[]
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    private void SelectPlanet()
    {
        isSelected = true;
        spriteRenderer.color = selectedColor;
        previousSelected = gameObject.GetComponent<Planet>();
    }

    private void DeselectedPlanet()
    {
        isSelected = false;
        spriteRenderer.color = Color.white;
        previousSelected = null;
    }

    //Lógica de la detección del touch
    bool CheckForTouch()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        { 
            var wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            var touchPosition = new Vector2(wp.x, wp.y);
 
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

    private void OnTouchDown()
    {
        if (spriteRenderer.sprite == null || BoardManager.sharedInstance.isShifting)
        {
            return;
        }

        if (isSelected)
        {
            DeselectedPlanet();
        }
        else
        {
            if (previousSelected == null)
            {
                SelectPlanet();
            }
            else
            {
                if (CanSwipe())
                {
                    SwapSprite(previousSelected);
                    previousSelected.FindAllMatches();
                    previousSelected.DeselectedPlanet();
                    FindAllMatches();

                    GUIManager.sharedInstance.MoveCounter--;
                }
                else
                {
                    previousSelected.DeselectedPlanet();
                    SelectPlanet(); 
                }
            }
        }        
    }

    public void SwapSprite(Planet newPlanet)
    {
        if (spriteRenderer.sprite == newPlanet.GetComponent<SpriteRenderer>().sprite)
        {
            return  ;
        }

        Sprite oldPlanet = newPlanet.spriteRenderer.sprite;
        newPlanet.spriteRenderer.sprite = this.spriteRenderer.sprite;
        this.spriteRenderer.sprite = oldPlanet;

        int tempId = newPlanet.id;
        newPlanet.id = this.id;
        this.id = tempId;
    }

    private GameObject GetNeighbor(Vector2 direccion)
    {
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direccion);
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }
    }

    private List<GameObject> GetAllNeighbors()
    {
        List<GameObject> neighbors = new List<GameObject>();

        foreach (Vector2 direction in adjacentDirections)
        {
            neighbors.Add(GetNeighbor(direction));
        }
        return neighbors;
    }

    private bool CanSwipe()
    {
        return GetAllNeighbors().Contains(previousSelected.gameObject);
    }

    private List<GameObject> FindMatch(Vector2 direction)
    {
        List<GameObject> matchingPlanets = new List<GameObject>();
        
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

        foreach (Vector2 direction in directions)
        {
            matchingPlanets.AddRange(FindMatch(direction));
        }

        if (matchingPlanets.Count >= BoardManager.MinPlanetsToMatch)
        {     
            //Si se hacen 4 en linea obtienes 1 movimiento y 10 puntos extra       
            if(matchingPlanets.Count == 3)
            {
                GUIManager.sharedInstance.MoveCounter += 1; 
                GUIManager.sharedInstance.Score += 10;  
            }

            //Si se hacen 5 en linea ganas 3 movimientos y 50 puntos extra
            if(matchingPlanets.Count == 4)
            {
                GUIManager.sharedInstance.MoveCounter += 3; 
                GUIManager.sharedInstance.Score += 50;   
            }
            
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
        bool hMatch = ClearMatch(new Vector2[2]
        {
            Vector2.left, Vector2.right });

        bool vMatch = ClearMatch(new Vector2[2]
       {
            Vector2.up, Vector2.down });

        if (hMatch || vMatch)
        {
            spriteRenderer.sprite = null;
            //BoardManager.sharedInstance.FindNullPlanets();

            StopCoroutine(BoardManager.sharedInstance.FindNullPlanets());
            StartCoroutine(BoardManager.sharedInstance.FindNullPlanets());
        }        
    }
}
