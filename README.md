# 実行方法 (Usage)



## 1. シミュレーション環境の起動 (MoveIt! Demo)

実機のロボットを接続せず、RViz上のみでSIA5の動作計画や軌道生成のシミュレーションを行う場合は、こちらのコマンドを使用します。

（※各コマンドは、それぞれ新しいターミナルを開いて実行してください）



**1. MoveIt! デモ環境の起動**
```bash
roslaunch motoman_moveit_config demo.launch
```

**2. SIA5 MoveIt! 環境の起動**
```bash
roslaunch sia5_moveit sia5_moveit.launch
```