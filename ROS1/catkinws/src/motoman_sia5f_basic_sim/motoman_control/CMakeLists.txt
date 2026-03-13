cmake_minimum_required(VERSION 3.0.2)
project(motoman_control)
find_package(catkin REQUIRED COMPONENTS
  roscpp
  actionlib
  actionlib_msgs
  trajectory_msgs
  control_msgs
  sensor_msgs
  message_generation 
  )

catkin_package(
  CATKIN_DEPENDS actionlib_msgs
  )

add_definitions("-std=c++11")

find_package(roslaunch)
# roslaunch_add_file_check(test/sia5_launch_test.xml)
# roslaunch_add_file_check(test/sia5_with_jamming_launch_test.xml)
# roslaunch_add_file_check(test/sia5_with_jamming_and_kinectv2_launch_test.xml)
# roslaunch_add_file_check(test/sia5_with_kinectv2_launch_test.xml)
# roslaunch_add_file_check(test/sia5_with_dhand_launch_test.xml)
# roslaunch_add_file_check(test/sia5_with_dhand_and_multi_kinect_launch_test.xml)

include_directories(
  ${catkin_INCLUDE_DIRS}
)

## Declare a cpp executable
add_executable(motoman_control_node
  src/motoman_control_node.cpp
)

## Add cmake target dependencies of the executable/library
## as an example, message headers may need to be generated before nodes
## *** make sure to run rosdep and install the uninstalled deps automatically
## add_dependencies(motoman_control_node motoman_control_generate_messages_cpp)
add_dependencies(motoman_control_node ${catkin_EXPORTED_TARGETS})

## Specify libraries to link a library or executable target against
target_link_libraries(motoman_control_node
  ${catkin_LIBRARIES}
)
