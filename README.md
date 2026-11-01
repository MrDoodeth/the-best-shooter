# The Best Shooter

A first-person zombie survival shooter built with Unity. Survive escalating enemy waves, collect resources, upgrade your combat options, and clear locations occupied by hostile zombies.

> **Work in progress:** the project is unfinished, but its core combat, enemy, inventory, and interaction systems are already implemented.

## 📦 Features

- First-person movement with sprinting, jumping, crouching, aiming, head bobbing, and weapon sway.
- Firearms and melee weapons, including pistols, SMGs, assault rifles, shotguns, sniper rifles, a knife, and a machete.
- Recoil, zoom, ammunition, reload, weapon switching, and a sniper scope UI.
- Configurable zombie waves with NavMesh-driven enemies and regular, shooter, explosive, toxic, sprinter, and tank variants.
- Pickups and temporary buffs for health, ammunition, fire damage, damage boosts, and slowdowns.
- Inventory, score, random item spawns, key/card-gated doors, point-gated doors, drawers, shelves, and a gun vendor.
- Universal Render Pipeline, VFX, audio, and UI for health, points, ammunition, and active buffs.

## 📋 Requirements

Unity `2021.3.25f1`.

## 🚀 Build and run

```bash
git clone https://github.com/MrDoodeth/the-best-shooter.git
cd the-best-shooter
```

Open the `Shooter` folder in Unity Hub with Unity `2021.3.25f1`.

The build settings include these scenes:

- `Assets/Scenes/SampleScene.unity`
- `Assets/Scenes/House.unity`

To play in the editor, open a scene and press **Play**. To create a build, open **File → Build Settings**, choose the target platform, make sure both scenes are included, then select **Build** or **Build And Run**.

## 🗂️ Project layout

```text
Shooter/
├── Assets/
│   ├── Scripts/         # Player, weapon, enemy, UI, and interaction systems
│   ├── Scenes/          # SampleScene and House
│   ├── Weapon/          # Weapon prefabs and animations
│   ├── Enemies/         # Zombie prefabs and presets
│   └── Stuff/           # Buffs, keys, cards, and other items
├── Packages/            # Unity package manifest and lock file
└── ProjectSettings/     # Unity project and build settings
```

## 📜 License

MIT, see [LICENSE](LICENSE).
