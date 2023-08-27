using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionTree : MonoBehaviour
{
    public GameObject Player;
    public LayerMask PlayerLayerMask;

    public LayerMask ObstacleLayerMask;

    public float Angle = 90f;
    public Animator myAnimator;

    public Maze PCGMaze;

    //these variables are used by AudioManager to verify if the enemy has changed types
    public AnimationType cur_anim_type;
    public AnimationType prev_anim_type;


    public enum AnimationType
    {
        WALKING, CHASING, IDLING, ATTACKING
    }

    private Leaf root = new Leaf();

    void Start()
    {
        cur_anim_type = AnimationType.IDLING;
        SetUpDecisionTree(root);
    }

    void Update()
    {
        Leaf currentLeaf = root;

        // Continue to iterate through children until we reach a leaf with no children.
        while(currentLeaf.ChildLeafs.Count > 0)
        {
            foreach(var child in currentLeaf.ChildLeafs)
            {
                if(child.CheckCondition(gameObject, Player))
                {
                    currentLeaf = child;
                    break;
                }
            }
        }
        currentLeaf.Action(Player, gameObject, 5f, myAnimator);
    }

    /// <summary>
    /// Manually sets up our decision tree.
    /// </summary>
    /// <param name="root"></param>
    void SetUpDecisionTree(Leaf root)
    {
        AttackNode attackNode = new AttackNode();
        attackNode.ParentTree = this;
        root.ChildLeafs.Add(attackNode);
        StunnedNode stunnedNode = new StunnedNode();
        root.ChildLeafs.Add(stunnedNode);
        ChaseNode chaseNode = new ChaseNode(PCGMaze);
        chaseNode.ParentTree = this;
        root.ChildLeafs.Add(chaseNode);

        IdleNode idleNode = new IdleNode(PCGMaze);
        idleNode.ParentTree = this;
        root.ChildLeafs.Add(idleNode);
    }


    public void PlayAnimation(AnimationType animationToPlay)
    {
        //this will help EnemyAudioManager know when a state has changed
        prev_anim_type = cur_anim_type;
        cur_anim_type = animationToPlay;
        switch (animationToPlay)
        {
            case AnimationType.ATTACKING:
                myAnimator.SetBool("IsIdle", false);
                myAnimator.SetBool("IsRunning", false);
                myAnimator.SetBool("IsWalking", false);
                myAnimator.SetBool("IsAttacking", true);
                break;
            case AnimationType.CHASING:
                myAnimator.SetBool("IsIdle", false);
                myAnimator.SetBool("IsRunning", true);
                myAnimator.SetBool("IsWalking", false);
                myAnimator.SetBool("IsAttacking", false);
                break;
            case AnimationType.IDLING:
                myAnimator.SetBool("IsIdle", true);
                myAnimator.SetBool("IsRunning", false);
                myAnimator.SetBool("IsWalking", false);
                myAnimator.SetBool("IsAttacking", false);
                break;
            case AnimationType.WALKING:
                myAnimator.SetBool("IsIdle", false);
                myAnimator.SetBool("IsRunning", false);
                myAnimator.SetBool("IsWalking", true);
                myAnimator.SetBool("IsAttacking", false);
                break;
        }
    }

    // TODO: I'll look into how to make the rotation smoother in the future.
    public void MoveAndRotate(Vector3 target, float rotationSpeed, float walkSpeed)
    {
        StopAllCoroutines();
        StartCoroutine(GradualMoveAndLookAt(target, rotationSpeed, walkSpeed));
    }

    private IEnumerator GradualMoveAndLookAt(Vector3 target, float rotationSpeed, float walkSpeed)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);

        float elapsedTime = 0f;
        while(elapsedTime < 1f)
        {
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime);
            elapsedTime += Time.deltaTime * rotationSpeed;
            transform.position = Vector3.MoveTowards(transform.position, target, walkSpeed * Time.deltaTime);
            yield return null;
        }

        transform.rotation = targetRotation;
    }

    private void OnDrawGizmos()
    {
        List<Node> path = ((IdleNode)root.ChildLeafs[root.ChildLeafs.Count - 1]).PathToGoal;

        if(path.Count > 0)
        {
            Node previousNode = path[0];

            for (int i = 1; i < path.Count; i++)
            {
                Gizmos.color = Color.green;

                Debug.Log($"Line Drawn on {previousNode.X}, {previousNode.Z}");
                Gizmos.DrawLine(new Vector3(previousNode.X, 10f, previousNode.Z), new Vector3(path[i].X, 10f, path[i].Z));
                previousNode = path[i];


                // Colors the goal block.
                if (i + 1 == path.Count)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireCube(new Vector3(path[i].X, 0f, path[i].Z), new Vector3(1f, 1f, 1f));
                }

            }
        }
    }
}
