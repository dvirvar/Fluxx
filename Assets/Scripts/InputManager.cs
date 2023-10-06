using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public event Action<Card> OnCardClicked = delegate { }; 
    [SerializeField] new Camera camera;
    [SerializeField] Board board;
    bool inverse, discardPileControl;
    int currentPileIndex;
    Vector3 cameraOrigin;
    private void Awake()
    {
        cameraOrigin = camera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (discardPileControl)
        {
            DiscardPileControl();
        } else
        {
            CameraMovement();
        }
        CheckClickPosition();
    }

    public void SetInverse(bool inverse)
    {
        this.inverse = inverse;
    }

    public void SetDiscardPileControl(bool discardPileControl)
    {
        this.discardPileControl = discardPileControl;
        if (discardPileControl) {
            camera.transform.position = board.GetDiscardPilePosition() + new Vector3(0,5,0);
            currentPileIndex = board.GetDiscardPileCards().Count - 1;
        } else
        {
            var pileCards = board.GetDiscardPileCards();
            for (; currentPileIndex < pileCards.Count; ++currentPileIndex) {
                pileCards[currentPileIndex].gameObject.SetActive(true);
            }
        }
    }

    public void ReturnToCameraOrigin()
    {
        camera.transform.position = cameraOrigin;
    }

    void CameraMovement()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            camera.transform.position = cameraOrigin;
        }
        else
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            var zoom = Input.GetAxis("Mouse ScrollWheel");
            if (inverse)
            {
                horizontal = -horizontal;
                vertical = -vertical;
                zoom = -zoom;
            }
            var deltaTime = Time.deltaTime;
            var cameraMovement = new Vector3(horizontal * deltaTime * 8, -zoom * deltaTime * 80, vertical * deltaTime * 8);
            if (cameraMovement != Vector3.zero)
            {
                var position = camera.transform.position + cameraMovement;
                position = new Vector3(Math.Clamp(position.x, -4, 4), Math.Clamp(position.y, 15, 25), Math.Clamp(position.z, -13, 13));
                camera.transform.position = position;
            }
        }
    }

    void DiscardPileControl()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            board.GetDiscardPileCards()[currentPileIndex].gameObject.SetActive(false);
            currentPileIndex = Math.Max(currentPileIndex - 1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            var pileCards = board.GetDiscardPileCards();
            var currentCard = pileCards[currentPileIndex];
            if (currentCard.gameObject.activeSelf)
            {
                currentPileIndex = Math.Min(currentPileIndex + 1, pileCards.Count - 1);
                pileCards[currentPileIndex].gameObject.SetActive(true);
            } else
            {
                pileCards[currentPileIndex].gameObject.SetActive(true);
                currentPileIndex = Math.Min(currentPileIndex + 1, pileCards.Count - 1);
            }
        }
    }

    void CheckClickPosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo))
            {
                var parent = hitInfo.collider.transform.parent;
                if (parent != null && parent.TryGetComponent<Card>(out var card) && card.CanBeSelected)
                {
                    OnCardClicked(card);
                }
            }
        }
    }
}
