#!/usr/bin/env python3

"""
Program description
    Testing motion planning using pilz motion planner package
"""

from geometry_msgs.msg import Point
from pilz_robot_programming import *

import math
import sys

import rospy

__REQUIRED_API_VERSION__ = "1"


def main():
    print("Executing " + __file__)
    robot = Robot(__REQUIRED_API_VERSION__)

    # step_tracing()
    
    # global vel scale
    global_vel_scale = 0.3

    
    joint_pick_home = [-1.5700054838223068, 5.800377889997321e-06, 9.502887525769665e-07, -3.8647429866145444e-05, 
                        1.0562285321924492e-05, -9.597806985439661e-05, -8.60341473885029e-06]
    
    # step_tracing()

    # go home
    robot.move(
        Ptp(
            goal=joint_pick_home,
            vel_scale=global_vel_scale
        )
    )

def step_tracing():
    """
    Step tracing the code when running
    """
    isContinue = input("Press enter to continue: ")
    if isContinue == "q":
        print("Stop program")
        sys.exit()
    else:
        print("Continue program")
        pass


if __name__ == "__main__":
    rospy.init_node("robot_program_node", anonymous=True)

    # step_tracing()
    main()