# RunNBlock - An ML Agents Unity Environment
The repository contains the unity environment scenes, prefabs and agent scripts required to replicate the results for training the algorithm.

## Setup
In order to recreate the project:
-Setup ml agents from the official repository. (https://github.com/Unity-Technologies/ml-agents/blob/release_19_docs/docs/Getting-Started.md)

-Startup a new Unity 3D Project.

-Add the ml agents toolkit from the packet manager.

-(optional)"WorldMaterialFree" asset bundle can also be downloaded from the unity asset store and be imported through the packet manager for visuals. (https://assetstore.unity.com/packages/2d/textures-materials/world-materials-free-150182)

-Drag and drop the Scenes, Prefabs and Scripts folders from the repository to the Assets folder under the project manager window.

-Copy the yaml folder from the repository to the config folder inside ml agents installation directory.

## Usage
-On the command prompt, navigate to the ml-agents installation directory
-Run the training with the following commands seperately for training with pure PPO and self play respectively:
```bash
  mlagents-learn config/Yaml/RunnerOnly.yaml --run-id=rollerAgent
  mlagents-learn config/Yaml/RunnerVsBlocker.yaml --run-id=rollerAgentVs
```
-Press the play button on the unity editor

-Run the following command for displaying results on tensorboard:
```bash
  tensorboard --logdir results
```
-Open a browser and go to http://localhost:6006/ for viewing the results.
