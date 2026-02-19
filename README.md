## 実行方法 (Usage)

本プロジェクトのROS環境を起動するには、用途に合わせて以下のいずれかのコマンドを実行してください。

### 1. シミュレーション環境の起動 (MoveIt! Demo)
実機のロボットを接続せず、RViz上のみでSIA5の動作計画や軌道生成のシミュレーションを行う場合は、こちらのコマンドを使用します。

```bash
roslaunch motoman_moveit_config demo.launch
```
```bash
roslaunch sia5_moveit sia5_moveit.launch
```
