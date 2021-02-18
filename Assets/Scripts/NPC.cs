using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NPC : NpcData
{
    public bool ShowDebugMessages;

    public NavMeshAgent agent { get; private set; }
    private Animator anim;

    [SerializeField] private float speedAnimDevider = 1;
    [SerializeField] private float stopDistance;
    [SerializeField] private float stopDistanceRandomAdjustment;
    public float VisionRange;
    public bool attacked;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();


        FindObjectOfType<DayAndNightControl>().OnMorningHandler += GoToWork;
        FindObjectOfType<DayAndNightControl>().OnEveningHandler += GoHome;

        stopDistance = Random.Range(stopDistanceRandomAdjustment + stopDistance, stopDistance);
        agent.stoppingDistance = stopDistance;

        GoHome();
    }

    void Update()
    {
        anim.SetFloat("InputMagnitude", agent.velocity.magnitude / speedAnimDevider);

        if ((currentState == NpcStates.GoingToWork && Vector3.Distance(transform.position, work.position) < stopDistance) || (agent.remainingDistance == 0 && !agent.pathPending))
        {
            if (ShowDebugMessages)
                Debug.Log("StartingToWork");
            currentState = NpcStates.Working;
            anim.SetBool("Working", true);
        }
        if (attacked)
        {
            anim.SetBool("Working", false);
            Escape();
        }
    }

    private void SetMoveTarget(Vector3 target)
    {
        agent.ResetPath();
        agent.SetDestination(target);
    }

    private void GoToWork()
    {
        if (currentState == NpcStates.GoingToWork)
            return;

        currentState = NpcStates.GoingToWork;
        SetMoveTarget(work.position);
        if (ShowDebugMessages)
            Debug.Log(name + " is going to work");
    }

    private void GoHome()
    {
        if (currentState == NpcStates.GoingHome)
            return;

        currentState = NpcStates.GoingHome;
        anim.SetBool("Working", false);

        SetMoveTarget(home.position);
        if (ShowDebugMessages)
            Debug.Log(name + " is going home");
    }
    private void Escape()
    {
        if (currentState == NpcStates.Escaping)
            return;
        currentState = NpcStates.Escaping;
        anim.SetFloat("InputMagnitude", 1);
        Vector3 dest = transform.position;
        dest = new Vector3(
                Random.Range(transform.position.x - VisionRange * 2, transform.position.x + VisionRange * 2),
                (transform.position.y),
                Random.Range(transform.position.z - VisionRange * 2, transform.position.z + VisionRange * 2)
                );
        Debug.Log(dest);
        SetMoveTarget(dest);
    }
    private void OnDestroy()
    {
        try
        {
            FindObjectOfType<DayAndNightControl>().OnMorningHandler -= GoToWork;
            FindObjectOfType<DayAndNightControl>().OnEveningHandler -= GoHome;
        }
        catch
        {
            if (ShowDebugMessages)
                Debug.LogWarning("DayAndNightControl object is not found. This is ok if the scene is unloaded.");
        }
    }
}
