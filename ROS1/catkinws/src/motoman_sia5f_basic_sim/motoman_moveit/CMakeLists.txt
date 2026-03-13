cmake_minimum_required(VERSION 3.0.2)
project(motoman_moveit)

find_package(catkin REQUIRED COMPONENTS
  motoman_description
  moveit_commander
  moveit_core
  moveit_msgs
  moveit_ros_planning
  moveit_ros_planning_interface
  moveit_python
  pluginlib
  roscpp
  roslaunch
)

find_package(Boost REQUIRED system filesystem date_time thread)
# find_package(Eigen REQUIRED)

catkin_package(
  CATKIN_DEPENDS roscpp moveit_msgs
  moveit_core
  moveit_ros_planning_interface
)

# Build
# include_directories(SYSTEM ${Boost_INCLUDE_DIR} ${EIGEN_INCLUDE_DIRS})
# include_directories(
#   include
#   ${catkin_INCLUDE_DIRS}
#   )

# include_directories(include ${catkin_INCLUDE_DIRS} ${Eigen_INCLUDE_DIRS})
