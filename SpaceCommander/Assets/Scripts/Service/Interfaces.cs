﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//using SpaceCommander.Units;

namespace SpaceCommander
{
    public interface ISpaceShipObservable
    {
        UnitClass Type { get; }
        float Hull { get; }
        float MaxHull { get; }
        float ShieldForce { get; }
        float ShieldCampacity { get; }
        SpellModule[] Module { get; }
        IWeapon[] PrimaryWeapon { get; }
        IWeapon[] SecondaryWeapon { get; }
        bool ManualControl { get; set; }
        Unit CurrentTarget { get; }
        Transform GetTransform();
    }
    public interface IArmor
    {
        float Hitpoints { get; set; }
        float MaxHitpoints { get; }
        float ShellResist { get; }
        float EnergyResist { get; }
        float BlastResist { get; }
        void StatUp(float hitpoints, float maxHitpoints, float shellResist, float energyResist, float blastResist);
    }
    public interface IShield
    {
        float Force { get; set; }
        float MaxCampacity { get; set; }
        float Recharging { get; set; }
        bool IsOverheat { get; }
        void Owerheat();
        void Reload();
        void Blink(float blink);
    }
    public interface IEngine
    {
        Transform transform { get; }
        IGunner Gunner { get; }
        float Speed { get; }
        float Acceleration { get; }
        float ShiftSpeed { get; }
        float RotationSpeed { get; }
        Vector3 ScaleJetream { set; get; }
    }
    public interface IDriver
    {
        Vector3 Velocity { get; }
        void Update();
        void FixedUpdate();
        bool MoveTo(Vector3 destination);
        bool MoveToQueue(Vector3 destination);
        bool MoveToQueue(Vector3[] path);
        bool Follow(IEngine target);
        bool ExecetePointManeuver(PointManeuverType type, Vector3 point, Vector3 direction);
        bool ExeceteTargetManeuver(TatgetManeuverType type, Transform target);
        void BuildPathArrows();
        void ClearQueue();
        int PathPoints { get; }
        DriverStatus Status { get; }
        Vector3 NextPoint { get; }
    }
    public interface IGunner
    {
        IWeapon[][] Weapon { get; }
        Unit Target { get; }
        void Update();
        bool SetAim(Unit target, bool immediately, float lockdown);
        bool ShootHim(int slot);
        bool Volley(int slot);
        bool ResetAim();
        float GetRange(int slot);
        bool SeeTarget();
        bool AimOnTarget();
        bool CanShoot(int slot);
        bool TargetInRange(int slot);
        bool NeedAim();
    }
    public interface IWeapon
    {
        WeaponType Type { get; }
        Unit Target { set; get; }
        float Firerate { get; } //per minute
        float Range { get; }
        float AimAngle { get; }
        float RoundSpeed { get; }
        float Dispersion { get; }
        float ShildBlink { get; }
        float BackCounter { get; }
        float ShootCounter { get; }
        float MaxShootCounter { get; }
        bool IsReady { get; }
        Transform transform { get; }
        void Reset();
        bool Fire();
    }
    public interface IShell
    {
        float Speed { get; }
        float Damage { get; }
        float ArmorPiersing { get; }
        void StatUp(Vector3 speed, float damage, float armorPiersing, float mass, bool canRicochet);
        void StatUp(Vector3 speed, float damage, float armorPiersing, float mass, bool canRicochet, GameObject explosionPrefab);
        void StatUp(Vector3 speed, float damage, float armorPiersing, float mass, bool canRicochet, GameObject explosionPrefab, float ttl);
    }
    public interface IEnergy
    {
        float Speed { get; }
        float Damage { get; }
        float ArmorPiersing { get; }
        void StatUp(EnergyType type);
        float GetEnergy();
    }
    public interface IImpact
    {
        string Name { get; }
        void ActImpact();
        void CompleteImpact();
        string ToString();
    }
}
