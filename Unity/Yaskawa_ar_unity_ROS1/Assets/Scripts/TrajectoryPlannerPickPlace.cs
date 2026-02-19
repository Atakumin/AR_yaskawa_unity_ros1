using System;
using System.Collections;
using System.Linq;
using System.Threading;
using RosMessageTypes.Geometry;
using RosMessageTypes.Sia5Moveit;
using RosMessageTypes.Trajectory;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using Unity.Robotics.UrdfImporter;
using UnityEngine;
//追加start
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
//追加end

public class TrajectoryPlannerPickPlace : MonoBehaviour
{
    // Hardcoded variables
    const int k_NumRobotJoints = 7;
    const float k_JointAssignmentWait = 0.05f;
    const float k_PoseAssignmentWait = 0.5f;

    // Variables required for ROS communication
    [SerializeField]
    string m_RosServiceName = "/robot_arm_moveit";
    public string RosServiceName { get => m_RosServiceName; set => m_RosServiceName = value; }

    [SerializeField]
    GameObject m_Sia5;
    public GameObject Sia5 { get => m_Sia5; set => m_Sia5 = value; }
    //[SerializeField]
    GameObject m_Target;
    public GameObject Target { get => m_Target; set => m_Target = value; }
    //[SerializeField]
    GameObject m_TargetPlacement;
    public GameObject TargetPlacement { get => m_TargetPlacement; set => m_TargetPlacement = value; }

    //追加srart
    private TextMeshProUGUI state_information;
    private TextMeshProUGUI robot_information;
    public GameObject StateInformation;
    public GameObject RobotInformation;
    //追加end


    // Assures that the gripper is always positioned above the m_Target cube before grasping.
    readonly Quaternion m_PickOrientation = Quaternion.Euler(0, 0, 0);
    readonly Vector3 m_PickPoseOffset2 = new Vector3(0.136f, 0.1f, 0.75f);
    readonly Vector3 m_PlacePoseOffset2 = new Vector3(0.136f, 0.1f, -1.048f); 

    readonly Vector3 m_PickPoseOffset1 = new Vector3(0, 0.24f, 0);
    readonly Vector3 m_PlacePoseOffset1 = new Vector3(0, 0.3f, 0);

    // Articulation Bodies
    ArticulationBody[] m_JointArticulationBodies;
    ArticulationBody m_gripperFinger1;
    ArticulationBody m_gripperFinger2;
    ArticulationBody m_gripperFinger3;
    ArticulationBody m_gripperFinger4;
    ArticulationBody m_gripperFinger5;
    ArticulationBody m_gripperFinger6;

    // ROS Connector
    ROSConnection m_Ros;

    //Takuya write
    public int NameLabel;
    /// <summary>
    ///     Find all robot joints in Awake() and add them to the jointArticulationBodies array.
    ///     Find left and right finger joints and assign them to their respective articulation body objects.
    /// </summary>


    void Start()
    {
        // Get ROS connection static instance
        m_Ros = ROSConnection.GetOrCreateInstance();
        m_Ros.RegisterRosService<MoverServiceRequest, MoverServiceResponse>(m_RosServiceName);

        m_JointArticulationBodies = new ArticulationBody[k_NumRobotJoints];

        var linkName = string.Empty;
        for (var i = 0; i < k_NumRobotJoints; i++)
        {
            linkName += SourceDestinationPublisher.LinkNames[i];
            m_JointArticulationBodies[i] = m_Sia5.transform.Find(linkName).GetComponent<ArticulationBody>();
        }


        // Find left and right fingers
        // var rightGripper = linkName + "/tool_link/gripper_base/servo_head/control_rod_right/right_gripper";
        var gripperFinger1 = linkName + "/tool0/gripper_finger_1";
        var gripperFinger2 = linkName + "/tool0/gripper_finger_2";
        var gripperFinger3 = linkName + "/tool0/gripper_finger_3";
        var gripperFinger4 = linkName + "/tool0/gripper_finger_4";
        var gripperFinger5 = linkName + "/tool0/gripper_finger_5";
        var gripperFinger6 = linkName + "/tool0/gripper_finger_6";

        // m_RightGripper = m_NiryoOne.transform.Find(rightGripper).GetComponent<ArticulationBody>();

        m_gripperFinger1 = m_Sia5.transform.Find(gripperFinger1).GetComponent<ArticulationBody>();
        m_gripperFinger2 = m_Sia5.transform.Find(gripperFinger2).GetComponent<ArticulationBody>();
        m_gripperFinger3 = m_Sia5.transform.Find(gripperFinger3).GetComponent<ArticulationBody>();
        m_gripperFinger4 = m_Sia5.transform.Find(gripperFinger4).GetComponent<ArticulationBody>();
        m_gripperFinger5 = m_Sia5.transform.Find(gripperFinger5).GetComponent<ArticulationBody>();
        m_gripperFinger6 = m_Sia5.transform.Find(gripperFinger6).GetComponent<ArticulationBody>();
        print("check1");

        //追加start
        robot_information = RobotInformation.GetComponent<TextMeshProUGUI>();
        robot_information.enabled = false;
        //追加end

    }

    /// <summary>
    ///     Close the gripper
    /// </summary>
    void CloseGripper()
    {
        var gripperFinger1Drive = m_gripperFinger1.xDrive;
        var gripperFinger2Drive = m_gripperFinger2.xDrive;
        var gripperFinger3Drive = m_gripperFinger3.xDrive;
        var gripperFinger4Drive = m_gripperFinger4.xDrive;
        var gripperFinger5Drive = m_gripperFinger5.xDrive;
        var gripperFinger6Drive = m_gripperFinger6.xDrive;


        gripperFinger1Drive.target = -25f;
        gripperFinger2Drive.target = -25f;
        gripperFinger3Drive.target = -25f;
        gripperFinger4Drive.target = -25f;
        gripperFinger5Drive.target = -25f;
        gripperFinger6Drive.target = -25f;


        m_gripperFinger1.xDrive = gripperFinger1Drive;
        m_gripperFinger2.xDrive = gripperFinger2Drive;
        m_gripperFinger3.xDrive = gripperFinger3Drive;
        m_gripperFinger4.xDrive = gripperFinger4Drive;
        m_gripperFinger5.xDrive = gripperFinger5Drive;
        m_gripperFinger6.xDrive = gripperFinger6Drive;
    }

    /// <summary>
    ///     Open the gripper
    /// </summary>
    void OpenGripper()
    {
        var gripperFinger1Drive = m_gripperFinger1.xDrive;
        var gripperFinger2Drive = m_gripperFinger2.xDrive;
        var gripperFinger3Drive = m_gripperFinger3.xDrive;
        var gripperFinger4Drive = m_gripperFinger4.xDrive;
        var gripperFinger5Drive = m_gripperFinger5.xDrive;
        var gripperFinger6Drive = m_gripperFinger6.xDrive;


        gripperFinger1Drive.target = 50f;
        gripperFinger2Drive.target = 50f;
        gripperFinger3Drive.target = 50f;
        gripperFinger4Drive.target = 50f;
        gripperFinger5Drive.target = 50f;
        gripperFinger6Drive.target = 50f;


        m_gripperFinger1.xDrive = gripperFinger1Drive;
        m_gripperFinger2.xDrive = gripperFinger2Drive;
        m_gripperFinger3.xDrive = gripperFinger3Drive;
        m_gripperFinger4.xDrive = gripperFinger4Drive;
        m_gripperFinger5.xDrive = gripperFinger5Drive;
        m_gripperFinger6.xDrive = gripperFinger6Drive;


    }

    //追加start
    public void PrintHideInformation()
    {
        robot_information = RobotInformation.GetComponent<TextMeshProUGUI>();
        if(robot_information.enabled == false)
        {
            robot_information.enabled = true;
        } else {
            robot_information.enabled = false;
        }
    }
    //追加end
   
    //追加start
    public void SelectTargets()
    {
        state_information = StateInformation.GetComponent<TextMeshProUGUI>();
        print("Raycheck0");
        RaycastHit hitTargets;
        print("Raycheck1");
        var ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        Debug.DrawRay(ray.origin, ray.direction, Color.red, 3.0f);

        print("Raycheck2");
        if (Physics.Raycast(ray, out hitTargets)) 
        {
            // ray処理
            if(hitTargets.collider.CompareTag("Target")){
                Target = hitTargets.collider.gameObject;
                print("SuccessSelectTarget");
                state_information.text = "Success Select TARGET";
            }else{
                print("FailSelectTarget");
                state_information.text = "Fail Select TARGET";
            }
        }
    }
    //追加end

    //追加start
    public void SelectTargetPlacements()
    {
        state_information = StateInformation.GetComponent<TextMeshProUGUI>();
        print("Raycheck3");
        RaycastHit hitTargetPlacements;
        print("Raycheck4");
        var ray2 = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        Debug.DrawRay(ray2.origin, ray2.direction, Color.red, 3.0f);

        print("Raycheck5");
        if (Physics.Raycast(ray2, out hitTargetPlacements)) 
        {
            // ray処理
            if(hitTargetPlacements.collider.CompareTag("TargetPlacement")){
                TargetPlacement = hitTargetPlacements.collider.gameObject;
                print("SuccessSelectTargetPlacement");
                state_information.text = "Success Select BOX";
            }else{
                print("FailSelectTargetPlacement");
                state_information.text = "Fail Select BOX";
            }
        }
    }


        
    //追加end



    /// <summary>
    ///     Get the current values of the robot's joint angles.
    /// </summary>
    /// <returns>NiryoMoveitJoints</returns>
    Sia5MoveitJointsMsg CurrentJointConfig()
    {
        print("check2");
        var joints = new Sia5MoveitJointsMsg();

        for (var i = 0; i < k_NumRobotJoints; i++)
        {
            joints.joints[i] = m_JointArticulationBodies[i].jointPosition[0];
        }

        return joints;
    }

    /// <summary>
    ///     Create a new MoverServiceRequest with the current values of the robot's joint angles,
    ///     the target cube's current position and rotation, and the targetPlacement position and rotation.
    ///     Call the MoverService using the ROSConnection and if a trajectory is successfully planned,
    ///     execute the trajectories in a coroutine.
    /// </summary>
    public void PublishJoints()
    {
        print("check5");
        var request = new MoverServiceRequest();
        request.joints_input = CurrentJointConfig();
        Debug.Log(request.joints_input);



        if (NameLabel == 1)
        {
            request.pick_pose = new PoseMsg
            {
                position = (m_Target.transform.position + m_PickPoseOffset1).To<FLU>(),

                // The hardcoded x/z angles assure that the gripper is always positioned above the target cube before grasping.
                orientation = Quaternion.Euler(0, m_Target.transform.eulerAngles.y, -90).To<FLU>()
            };
            request.place_pose = new PoseMsg
            {
                position = (m_TargetPlacement.transform.position + m_PlacePoseOffset1).To<FLU>(),
                orientation = Quaternion.Euler(0, m_Target.transform.eulerAngles.y, -90).To<FLU>()//m_PickOrientation.To<FLU>()
            };
            print("name1");
        }
        else if (NameLabel == 2)
        {
            request.pick_pose = new PoseMsg
            {
                position = (m_Target.transform.position + m_PickPoseOffset2).To<FLU>(),

                // The hardcoded x/z angles assure that the gripper is always positioned above the target cube before grasping.
                orientation = Quaternion.Euler(90, m_Target.transform.eulerAngles.y, 0).To<FLU>()
            };
            request.place_pose = new PoseMsg
            {
                position = (m_TargetPlacement.transform.position + m_PlacePoseOffset2).To<FLU>(),
                orientation = m_PickOrientation.To<FLU>()
            };
            print("name2");
        }
        // Place Pose

        /*position = (m_TargetPlacement.transform.position + m_PlacePoseOffset1).To<FLU>(),
        orientation = m_PickOrientation.To<FLU>()*/

        m_Ros.SendServiceMessage<MoverServiceResponse>(m_RosServiceName, request, TrajectoryResponse);
        print("test1");
    }

    void TrajectoryResponse(MoverServiceResponse response)
    {
        print("test2");
        if (response.trajectories.Length > 0)
        {
            Debug.Log("Trajectory returned.");
            StartCoroutine(ExecuteTrajectories(response, true));
        }
        else
        {
            Debug.LogError("No trajectory returned from MoverService.");
        }
        // this.GetComponent<TrajectoryPlanner2>().PublishJoints();
    }

    /// <summary>
    ///     Execute the returned trajectories from the MoverService.
    ///     The expectation is that the MoverService will return four trajectory plans,
    ///     PreGrasp, Grasp, PickUp, and Place,
    ///     where each plan is an array of robot poses. A robot pose is the joint angle values
    ///     of the six robot joints.
    ///     Executing a single trajectory will iterate through every robot pose in the array while updating the
    ///     joint values on the robot.
    /// </summary>
    /// <param name="response"> MoverServiceResponse received from niryo_moveit mover service running in ROS</param>
    /// <returns></returns>


    IEnumerator ExecuteTrajectories(MoverServiceResponse response, bool includeReturnTrajectory)
    {


        if (response.trajectories != null)
        {
            // For every trajectory plan returned
            state_information = StateInformation.GetComponent<TextMeshProUGUI>();
            print("Start Simulation");
            state_information.text = "Start Simulation";
            for (var poseIndex = 0; poseIndex < response.trajectories.Length; poseIndex++)
            {
                // For every robot pose in trajectory plan
                foreach (var t in response.trajectories[poseIndex].joint_trajectory.points)
                {
                    var jointPositions = t.positions;
                    var result = jointPositions.Select(r => (float)r * Mathf.Rad2Deg).ToArray();

                    // Set the joint values for every joint
                    for (var joint = 0; joint < m_JointArticulationBodies.Length; joint++)
                    {
                        var joint1XDrive = m_JointArticulationBodies[joint].xDrive;
                        joint1XDrive.target = result[joint];
                        m_JointArticulationBodies[joint].xDrive = joint1XDrive;
                    }

                    // Wait for robot to achieve pose for all joint assignments
                    yield return new WaitForSeconds(k_JointAssignmentWait);
                }

                if (poseIndex == (int)Poses.Firstinit)
                {
                    //yield return new WaitForSeconds(2.0f);
                }


                // Close the gripper if completed executing the trajectory for the Grasp pose
                if (poseIndex == (int)Poses.PreGrasp)
                {
                    CloseGripper();
                }

                // Wait for the robot to achieve the final pose from joint assignment
                yield return new WaitForSeconds(k_PoseAssignmentWait);

                if (poseIndex == (int)Poses.Place)
                {
                    //yield return new WaitForSeconds(4.3f);
                    OpenGripper();
                    yield return new WaitForSeconds(1.0f);
                }
            }

            // All trajectories have been executed, open the gripper to place the target cube
            print("Finish Simulation");
            state_information.text = "Finish Simulation";

            print("Select Target & Box");
            state_information.text = "Select Target & Box";
        }
    }




    enum Poses
    {
        Firstinit,
        PreGrasp,
        Grasp,
        PickUp,
        Place,
        Returninit
    }
}