using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationScript : MonoBehaviour
{
    private enum CustomerState
    {
        Searching,
        Shopping,
        Checkout,
        Exiting
    }

    [SerializeField]
    private List<Transform> needPoints; // Points for different needs like bakery, juices, etc.
    [SerializeField]
    private Transform cashierPoint; // Point for the cashier
    [SerializeField]
    private Transform exitPoint; // Point for the store exit

    private NavMeshAgent agent;
    private Animator animator;
    private CustomerState currentState = CustomerState.Searching;
    private float waitTimeAtShelf = 2.0f; // Time to wait at the shelf
    private float waitTimeAtCheckout = 3.0f; // Time to wait at the checkout
    private float waitTimer;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // Ensure the GameObject has an Animator component
        animator.SetBool("isMoving", true);
        ChooseNextNeed();
    }

    // Update is called once per frame
    void Update()
    {

        switch (currentState)
        {
            case CustomerState.Searching:
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    currentState = CustomerState.Shopping;
                    waitTimer = waitTimeAtShelf;
                }
                break;
            case CustomerState.Shopping:
                if (waitTimer > 0)
                {
                    waitTimer -= Time.deltaTime;
                    animator.SetBool("isMoving", false); // Customer is not moving while shopping
                }
                else
                {
                    currentState = CustomerState.Checkout;
                    waitTimer = waitTimeAtCheckout; // Initialize to 3 seconds when customer reaches the cashier
                    animator.SetBool("isMoving", true);
                    agent.destination = cashierPoint.position;
                }
                break;
            case CustomerState.Checkout:
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    if (waitTimer > 0)
                    {
                        waitTimer -= Time.deltaTime;
                        animator.SetBool("isMoving", false); // Customer is not moving while checking out
                    }
                    else
                    {
                        currentState = CustomerState.Exiting;
                        animator.SetBool("isMoving", true);
                        agent.destination = exitPoint.position;
                    }
                }
                break;
            case CustomerState.Exiting:
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    animator.SetBool("isMoving", false); // Customer stops moving at the exit
                    gameObject.SetActive(false); // Or reset the customer for reuse
                }
                break;
        }

    }

    private void ChooseNextNeed()
    {
        // Choose a random need
        int needIndex = Random.Range(0, needPoints.Count);
        agent.destination = needPoints[needIndex].position;
    }
}
