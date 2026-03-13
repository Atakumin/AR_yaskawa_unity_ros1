# Simulation ROS Package for MOTOMAN SIA5F Robot

To use the simulation, you need to create a new worksp and clone this repository and [this](https://github.com/hayashilab/motoman) then use the build tool to build this 2 metapackages

## Directory Structure
```
motoman_sim_ws
    |— build
    |— devel
    |— logs
    |— src
        |— motoman
        |— motoman_sia5f_basic_sim
```

After you build a package, you can run the simulation by
```
roslaunch motoman_moveit sia5f_basic_moveit_sim.launch
```
The simulation should start (RViz+Gazebo) and you can use for your research

In the case you want to modify, you can modify the robot description and gazebo world as well