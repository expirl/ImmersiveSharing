# ImmersiveSharing
For a detailed, Please see the paper : **[DBpia](https://www.dbpia.co.kr/journal/articleDetail?nodeId=NODE12131686) [ResearchGate](https://www.researchgate.net/publication/388960582_Enhancing_the_Experience_Sharing_through_User_Attention_HeatMaps_and_3D_Gaussian_Splatting_in_Virtual_Reality)**

I plan to provide my space dataset which is pre-made Gaussian Splats asset : **[Please see the links here.](https://drive.google.com/drive/folders/1rA-D_rgtHcfP4HGZrl52wFrA_k-gpFRK?usp=sharing)**

🎥 [Detailed Demo Video.](https://www.youtube.com/watch?v=FXJdRBDzgeY)

## Introduction
- This repository is code of my Undergrad research paper "Enhancing the Experience Sharing through User Attention HeatMaps and 3D Gaussian Splatting in Virtual Reality".
- It aims to enhance communication between users by constructing immersive VR spaces using 3D reconstruction and visualizing shared gaze data through heatmaps.

## Demo - Sharer
https://github.com/user-attachments/assets/73303b73-ef4c-4ee9-9408-ffc002ae70e8

https://github.com/user-attachments/assets/3c7b1ada-44c8-47bb-bb10-744754e6acea

## Demo - Receiver
https://github.com/user-attachments/assets/a02900f9-57f1-4b95-852e-79e5d0ab8b97


 ## Environment with Develop
- OpenXR & XR Interaction Toolkit
- DataBase : Firebase Realtime Database
- Unity 2022.3.7f1

 ## What Features Developed?
 [Compared to the previous project.](https://youtu.be/bYPWlAqcOMY)
- Updated gaze data collection.
- Transfer and Receive Data from FireBase RealTimeBase.
- Visualize a heatmap based on the data you've been sent.
- Gaze Reticle instead of Ray.
- Change Transparent Material.
- Add HandMenu UI

 ## Update Code.
- [GazeInteractor.cs](https://github.com/expirl/ImmersiveSharing/blob/main/Assets/GazeInteraction/Scripts/GazeInteractor.cs)
- [HeadGazeHeatMap.cs](https://github.com/expirl/ImmersiveSharing/blob/main/Assets/Scripts/HeadGaze/HeadGazeHeatmap.cs)
- [FireBaseHeatMapVisualization.cs](https://github.com/expirl/ImmersiveSharing/blob/main/Assets/Scripts/Firebase/FirebaseHeatmapVisualization.cs)
- [AnimateHandOnInput.cs](https://github.com/expirl/ImmersiveSharing/blob/main/Assets/Scripts/AnimateHandOnInput.cs)
- [TransparentHeatMap.shader](https://github.com/expirl/ImmersiveSharing/blob/main/Assets/Shader/TransparentHeatMap.shader)

 ## Pre-check.
![image](https://github.com/user-attachments/assets/977cf570-8976-47ab-9cbd-bd33bd1e15d3)

- For Gaussian visualization, Clone the original `UnityGaussianProject` and Copy the package file inside it to the location where the error was found. [UnityGaussianProject](https://github.com/aras-p/UnityGaussianSplatting)

- To use the FireBase database, Get your google-services.json and Put it in StreamingAssets, and Don't forget to download the FireBase Unity SDK. [FIREBASE TUTORIAL](https://www.youtube.com/watch?v=hAa5exkTsKI&t=344s)

- If you want the Demo-like environment, Please download the Gaussian Dataset from the link above.

 ## References
- [3D Gaussian Splatting](https://github.com/graphdeco-inria/gaussian-splatting)
- [UnityGaussianProject](https://github.com/aras-p/UnityGaussianSplatting)
- [HeatMap Shader](https://github.com/ericalbers/UnityHeatmapShader?tab=readme-ov-file)
- [Gaze Reticle](https://assetstore.unity.com/packages/tools/camera/vr-gaze-interaction-system-241337)
- [HandMenu UI](https://www.youtube.com/watch?v=6PSLfRsN89g)

 ## Citation
 If you use this repo in your research, please consider citing the paper as follows:
```bibtex
@inproceedings{kim2025ImmersiveSharing,
  author    = {KyoungTae Kim and SeongKi Kim},
  title     = {Enhancing the Experience Sharing through User Attention HeatMaps and 3D Gaussian Splatting in Virtual Reality},
  booktitle = {Proceedings of HCI KOREA 2025},
  year      = {2025},
  url       = {https://www.dbpia.co.kr/journal/articleDetail?nodeId=NODE12131686},
}
