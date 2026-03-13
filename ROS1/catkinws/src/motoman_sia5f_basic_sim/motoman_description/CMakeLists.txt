cmake_minimum_required(VERSION 3.0.2)
project(motoman_description)

find_package(catkin REQUIRED COMPONENTS
  joint_state_publisher
  robot_state_publisher
  roslaunch
  rviz
  xacro
)

catkin_package(
#  INCLUDE_DIRS include
#  LIBRARIES motoman_description
#  CATKIN_DEPENDS joint_state_publisher robot_state_publisher roslaunch rviz xacro
#  DEPENDS system_lib
)

include_directories(
  # include
  ${catkin_INCLUDE_DIRS}
)
