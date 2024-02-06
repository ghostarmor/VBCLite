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
    private List<Transform> cashierPoints; // Now a list of cashier points
    [SerializeField]
    private Transform exitPoint; // Point for the store exit

    private Transform currentTarget; // Currently targeted shelf

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

        if (currentState == CustomerState.Shopping && waitTimer > 0)
        {
            //LookAtTarget(currentTarget.position);
            //agent.updateRotation = true;
        }

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
                    LookAtTarget(currentTarget.position);
                    waitTimer -= Time.deltaTime;
                    animator.SetBool("isMoving", false); // Customer is not moving while shopping
                }
                else
                {
                    GoToCheckout();
                    //agent.destination = currentTarget.position;
                    currentState = CustomerState.Checkout;
                    waitTimer = waitTimeAtCheckout; // Initialize to 3 seconds when customer reaches the cashier
                    animator.SetBool("isMoving", true);
                    
                }
                break;
            case CustomerState.Checkout:
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    if (waitTimer > 0)
                    {
                        LookAtTarget(currentTarget.position);
                        waitTimer -= Time.deltaTime;
                        animator.SetBool("isMoving", false); // Customer is not moving while checking out
                    }
                    else
                    {
                        agent.destination = exitPoint.position;
                        currentState = CustomerState.Exiting;
                        animator.SetBool("isMoving", true);
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
        currentTarget = needPoints[needIndex]; // Set the current target
        agent.destination = currentTarget.position;
    }

    private void LookAtTarget(Vector3 targetPosition)
    {
        // Calculate the direction from the character to the target
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Ensure we only rotate around the y-axis
        direction.y = 0;

        // Check if we have a valid direction
        if (direction != Vector3.zero)
        {
            // Create a rotation that looks in the direction of the target
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            // Since the pivot is in the center, we ensure that the rotation is only around the y-axis
            lookRotation.x = 0;
            lookRotation.z = 0;

            // Smoothly rotate towards the target over time
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * agent.angularSpeed);
        }
    }

    private void GoToCheckout()
    {
        // Find the nearest cashier from the list
        Transform nearestCashier = FindNearestCashier();
        currentTarget = nearestCashier;
        agent.destination = nearestCashier.position; // Set the agent's destination to the nearest cashier
        // Assuming currentTarget should be updated as well
        currentTarget = nearestCashier;
    }

    private Transform FindNearestCashier()
    {
        Transform nearest = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        // Iterate through all cashiers to find the nearest one
        foreach (Transform cashier in cashierPoints)
        {
            float distance = Vector3.Distance(cashier.position, currentPosition);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = cashier;
            }
        }

        return nearest;
    }
}
