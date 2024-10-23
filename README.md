# Sharing Heatmaps between users in Reconstructed VR space
 모든 시행착오 공유 레포지토리

 ## Tech Stack
- OpenXR & XR Interaction Toolkit
- DataBase : Firebase Realtime Database
- Unity 2022.3.7f1

 ## What Features Developed?
 [Compared to the previous repository.](https://github.com/KimGyoungTae/3DGaussian-VR-HeatMap)
- Updated gaze data collection.
- Transfer and Receive Data from FireBase RealTimeBase.
- Visualize a heatmap based on the data you've been sent.
- Gaze Reticle instead of Ray.
- Change Transparent Material.
- Add HandMenu UI

 ## Update Code.
- GazeInteractor.cs
- HeadGazeHeatMap.cs
- FireBaseHeatMapVisualization.cs
- AnimateHandOnInput.cs
- TransparentHeatMap.shader

 ## Pre-check.
![image](https://github.com/user-attachments/assets/322b5c14-cb77-40a0-bd33-9042a69a8503)
- For Gaussian visualization, Clone the original `UnityGaussianProject` and Copy the package file inside it to the location where the error was found. [UnityGaussianProject](https://github.com/aras-p/UnityGaussianSplatting)

- To use the FireBase database, Get your google-services.json and Put it in StreamingAssets, and Don't forget to download the FireBase Unity SDK. [FIREBASE TUTORIAL](https://www.youtube.com/watch?v=hAa5exkTsKI&t=344s)
  
 ## References
- [3D Gaussian Splatting](https://github.com/graphdeco-inria/gaussian-splatting)
- [UnityGaussianProject](https://github.com/aras-p/UnityGaussianSplatting)
- [HeatMap Shader](https://github.com/ericalbers/UnityHeatmapShader?tab=readme-ov-file)
- [Gaze Reticle](https://assetstore.unity.com/packages/tools/camera/vr-gaze-interaction-system-241337)
