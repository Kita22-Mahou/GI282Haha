BOX MERGE - SCRIPT SETUP
========================

FILES
-----
Box.cs
BoxDatabase.cs
BoxQueue.cs
BoxSpawner.cs
GameManager.cs
ScoreUI.cs
NextBoxUI.cs

PREFABS
-------
Create 8 separate prefabs:
Box_Lv1.prefab
Box_Lv2.prefab
Box_Lv3.prefab
Box_Lv4.prefab
Box_Lv5.prefab
Box_Lv6.prefab
Box_Lv7.prefab
Box_Lv8.prefab

Each Box prefab should contain:
- SpriteRenderer
- BoxCollider2D
- Rigidbody2D
- Box.cs

Recommended Rigidbody2D:
- Body Type: Dynamic
- Collision Detection: Continuous
- Interpolate: Interpolate

Each prefab gets its own Level in Box.cs:
Lv1 = 1 ... Lv8 = 8

SCENE OBJECTS
-------------
1) BoxDatabase
Add BoxDatabase.cs.
Set Box Prefabs size to 8.
Element 0 = Box_Lv1
Element 1 = Box_Lv2
...
Element 7 = Box_Lv8

2) BoxQueue
Add BoxQueue.cs.
Suggested:
Min Level = 1
Max Level = 3
Preview Count = 3

3) BoxSpawner
Add BoxSpawner.cs.
Assign:
- Database = BoxDatabase
- Box Queue = BoxQueue
- Next Box UI = object with NextBoxUI.cs

Suggested:
Min X = -2.5
Max X = 2.5
Spawn Y = 5.5
Move Speed = 2.5
Next Spawn Delay = 0.4

4) GameManager
Add GameManager.cs.

5) Canvas
Score:
- Create TextMeshPro text for the score.
- Add ScoreUI.cs to an object.
- Assign Score Text.
- Assign that ScoreUI component to GameManager.

Next:
- Create a right-side panel.
- Add 3 UI Image objects as preview slots.
- Add NextBoxUI.cs to the panel (or another UI object).
- Assign the 3 Image components to Preview Images in order.

IMPORTANT
---------
BoxQueue is responsible for random levels.
BoxSpawner consumes one level from the queue to spawn the current box.
NextBoxUI shows the remaining queue.
Spacebar drops the current box.
Same-level boxes merge into the next level prefab.
Level 8 does not merge further.

INPUT SYSTEM
------------
This code uses the new Unity Input System:
UnityEngine.InputSystem.Keyboard

Do not use Input.GetKey, Input.GetMouseButton, or Input.touchCount
in these scripts.

IF SCORE DOES NOT UPDATE
-----------------------
Check:
- GameManager exists in the scene.
- GameManager has ScoreUI assigned.
- ScoreUI has its TMP text assigned.
- Box.cs calls GameManager.Instance.AddScore() on merge.
