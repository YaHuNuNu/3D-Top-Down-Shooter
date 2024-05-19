# 3D Top Down Shooter

## 展示

视频：[Unity学习展示 3D俯视角射击 (bilibili.com)](https://www.bilibili.com/video/BV11fueewEKL/?vd_source=2645cb7b5e7db8e5651a17ba55365bd1)

<br>

<div align="center">
<img src=".\READMEAssets\MainMenu.png" alt="MainMenu" width="500" />
</div>

<br>

<div align="center">
<img src=".\READMEAssets\Playing_1.png" alt="Playing_1" width="500" />
</div>

<br>

<div align="center">
<img src=".\READMEAssets\Playing_2.png" alt="Playing_2" width="500" />
</div>

<br>

## 限制

使用Unity Official releases 2022版及以上

<br>

## 介绍

<br>

动画控制没有用Animator组件，而是用 Playables API 写了一个简易的动画控制器，支持1D和2D动画混合、动画过渡、Avatar Mask以及与Animator Override Controller类似的功能，配合状态机框架实现对玩家和敌人动画状态的控制。 对应的代码位于 Assets\Framework\FSM 中。

参照 QFramework 写的事件系统、MVC框架、对象池、Command，以及这些模块底层的IOCContainer。对应的代码位于 Assets\Framework\CoreFramework 中。

<br>

## 参考

[mixamo](https://www.mixamo.com/)

[QFramework](https://qframework.cn/qf)

[The Complete Guide to Unity 3D : Making an Action Shooter](https://www.udemy.com/course/3d-tds-alexdev/?couponCode=LEADERSALE24A)
