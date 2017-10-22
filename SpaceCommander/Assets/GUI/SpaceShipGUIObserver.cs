﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using DeusUtility.UI;

namespace SpaceCommander
{
    public enum ObserverMode { None, Half, Full }
    public class SpaceShipGUIObserver : MonoBehaviour
    {
        //public Texture2D healthPanel;
        //public Texture2D healthBarBack;
        //public Texture2D healthBarFill;
        //public Texture2D shieldBarBack;
        //public Texture2D shieldBarFill;
        private const float MovDuratuon = 0.8f;
        private Image ShieldBar;
        private Text ShieldCount;
        private Image HealthBar;
        private Text HealthCount;

        //weapon
        public Texture CanonIcon;
        public Texture AutocannonIcon;
        public Texture ShotCannonIcon;
        public Texture RailgunIcon;
        public Texture RailmortarIcon;
        public Texture LaserIcon;
        public Texture PlasmaIcon;
        public Texture MagnetoIcon;
        public Texture MissileIcon;
        public Texture TorpedoIcon;
        //Module
        public Texture DefaultSpellIcon;
        public Texture MissoleTrapSpellIcon;
        public Texture JammerSpellIcon;
        public Texture TransponderSpellIcon;
        public Texture WarpSpellIcon;
        public Texture RadarBoosterSpellIcon;
        public Texture TrusterStunnerSpellIcon;
        public Texture ShieldStunnerSpellIcon;
        public Texture EmergencySelfRapairingSpellIcon;
        public Texture EmergencyShieldRechargingSpellIcon;
        public Texture AcceleratingCoilsSpellIcon;
        public Texture RechargeAcceleratorPassiveSpellIcon;
        public Texture ExtendedAmmoPackSpellIcon;
        public Texture TeamSpiritSpellIcon;
        public Texture ForcedTargetDesignatorSpellIcon;

        public ISpaceShipObservable observable;
        private HUDBase hud;
        private GlobalController Global;
        public GUIStyle localStyle;
        private float statusCount;
        private bool statusIsOpen;
        private float previevCount;
        private bool previevIsOpen;
        private GameObject canvas;
        private GameObject status;
        private GameObject previev;
        private GameObject spellPanel;
        private GameObject weaponPanel;
        private GameObject primaryWeaponSlot;
        private GameObject secondaryWeaponSlot;
        private Image[] PrimaryCooldown;
        private Text[] PrimaryCounters;
        private Image[] SecondaryCooldown;
        private Text[] SecondaryCounters;
        private Image[] moduleCooldown;
        private Image[] moduleActive;
        private ObserverMode mode;
        private float statusOpenedY;
        private float previevOpenedX;
        private OrbitalCamera maincam;


        private void OnEnable()
        {
            statusCount = MovDuratuon;
            statusIsOpen = true;
            previevCount = MovDuratuon;
            previevIsOpen = true;
            maincam = FindObjectOfType<OrbitalCamera>();
            hud = FindObjectOfType<HUDBase>();
            Global = FindObjectOfType<GlobalController>();
            canvas = GameObject.Find("Canvas");
            status = GameObject.Find("StatusPanel");
            statusOpenedY = status.transform.position.y;
            previev = GameObject.Find("UnitPreviewPanel");
            previevOpenedX = previev.transform.position.x;
            weaponPanel = GameObject.Find("WeaponPanel");
            primaryWeaponSlot = weaponPanel.transform.Find("PrimarySlot").gameObject;
            secondaryWeaponSlot = weaponPanel.transform.Find("SecondarySlot").gameObject;
            spellPanel = GameObject.Find("SpellButtons");
            ShieldBar = GameObject.Find("ShieldBar").GetComponent<Image>();
            ShieldCount = GameObject.Find("ShieldCount").GetComponent<Text>();
            HealthBar = GameObject.Find("HealthBar").GetComponent<Image>();
            HealthCount = GameObject.Find("HealthCount").GetComponent<Text>();
        }
        public void SetObservable(ISpaceShipObservable observable)
        {
            this.observable = observable;
            mode = ObserverMode.Half;
            float scaleLocal = hud.scale / 1.5f;
            //create primary weapon observ
            {
                GameObject primOrigin = primaryWeaponSlot.transform.GetChild(0).gameObject;
                //destroy children
                for (int i = primaryWeaponSlot.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(primaryWeaponSlot.transform.GetChild(i).gameObject);
                }
                //create new children
                GameObject newChild;
                Texture newTexture;

                PrimaryCooldown = new Image[observable.PrimaryWeapon.Length];
                PrimaryCounters = new Text[observable.PrimaryWeapon.Length];
                for (int i = 0; i < observable.PrimaryWeapon.Length; i++)
                {
                    newChild = Instantiate(primOrigin, new Vector3(primOrigin.transform.position.x + 100 * i * scaleLocal, primOrigin.transform.position.y, primOrigin.transform.position.z), primOrigin.transform.rotation, primaryWeaponSlot.transform);

                    newTexture = IconOf(observable.PrimaryWeapon[i].Type);

                    newChild.GetComponent<RawImage>().enabled = true;
                    newChild.transform.FindChild("Icon").GetComponent<RawImage>().texture = newTexture;
                    newChild.transform.FindChild("Icon").GetComponent<RawImage>().enabled = true;
                    newChild.transform.FindChild("Overlay").GetComponent<RawImage>().enabled = true;
                    PrimaryCooldown[i] = newChild.GetComponentInChildren<Image>();
                    newChild.GetComponentInChildren<Image>().enabled = true;
                    PrimaryCounters[i] = newChild.GetComponentInChildren<Text>();
                    newChild.GetComponentInChildren<Text>().enabled = true;
                }
                //secondary
                {
                    GameObject secOrigin = secondaryWeaponSlot.transform.GetChild(0).gameObject;
                    //destroy children
                    for (int i = secondaryWeaponSlot.transform.childCount - 1; i >= 0; i--)
                    {
                        Destroy(secondaryWeaponSlot.transform.GetChild(i).gameObject);
                    }

                    SecondaryCooldown = new Image[observable.SecondaryWeapon.Length];
                    SecondaryCounters = new Text[observable.SecondaryWeapon.Length];
                    for (int i = 0; i < observable.SecondaryWeapon.Length; i++)
                    {
                        newChild = Instantiate(secOrigin, new Vector3(secOrigin.transform.position.x + 100 * i * scaleLocal, secOrigin.transform.position.y, secOrigin.transform.position.z), secOrigin.transform.rotation, secondaryWeaponSlot.transform);

                        newTexture = IconOf(observable.SecondaryWeapon[i].Type);

                        newChild.GetComponent<RawImage>().enabled = true;
                        newChild.transform.FindChild("Icon").GetComponent<RawImage>().texture = newTexture;
                        newChild.transform.FindChild("Icon").GetComponent<RawImage>().enabled = true;
                        newChild.transform.FindChild("Overlay").GetComponent<RawImage>().enabled = true;
                        SecondaryCooldown[i] = newChild.GetComponentInChildren<Image>();
                        newChild.GetComponentInChildren<Image>().enabled = true;
                        SecondaryCounters[i] = newChild.GetComponentInChildren<Text>();
                        newChild.GetComponentInChildren<Text>().enabled = true;
                    }
                }
            }
            //module observ
            {
                GameObject modulOrigin = spellPanel.transform.GetChild(0).gameObject;
                int i = 0;
                //destroy children
                for (i = spellPanel.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(spellPanel.transform.GetChild(i).gameObject);
                }
                //create new children
                GameObject newChild;
                Texture newTexture;

                moduleCooldown = new Image[observable.Module.Length];
                moduleActive = new Image[observable.Module.Length];
                float xPos;
                for (i = 0; i < observable.Module.Length; i++)
                {
                    xPos = -(80f * (observable.Module.Length - 1f)) / 2f + (80f * i);
                    newChild = Instantiate(modulOrigin, Vector3.zero, modulOrigin.transform.rotation, spellPanel.transform);

                    newChild.transform.localPosition = new Vector3(xPos, modulOrigin.transform.localPosition.y, modulOrigin.transform.localPosition.z);

                    newTexture = IconOf(observable.Module[i].GetType());

                    newChild.GetComponent<Image>().enabled = true;
                    newChild.transform.FindChild("Icon").GetComponent<RawImage>().texture = newTexture;
                    newChild.transform.FindChild("Icon").GetComponent<RawImage>().enabled = true;

                    newChild.GetComponent<Button>().enabled = true;
                    newChild.GetComponent<Button>().onClick.RemoveAllListeners();
                    AddListener(newChild.GetComponent<Button>(), i);
                    //UnityEngine.Events.UnityAction action = () => { this.ActiveModule((int)i); };
                    //newChild.GetComponent<Button>().onClick.AddListener(action);

                    moduleActive[i] = newChild.transform.FindChild("ActiveOverlay").GetComponent<Image>();
                    moduleActive[i].enabled = true;
                    moduleCooldown[i] = newChild.transform.FindChild("CooldownOverlay").GetComponent<Image>();
                    moduleCooldown[i].enabled = true;
                }
            }
        }
        private void AddListener(Button but, int param)
        {
            but.onClick.AddListener(delegate () { ActiveModule((int)param); });
        }

        private Texture IconOf(WeaponType type)
        {
            switch (type)
            {

                case WeaponType.MachineCannon:
                    {
                        return this.AutocannonIcon;
                    }
                case WeaponType.ShootCannon:
                    {
                        return this.ShotCannonIcon;
                    }
                case WeaponType.Railgun:
                    {
                        return this.RailgunIcon;
                    }
                case WeaponType.Railmortar:
                    {
                        return this.RailmortarIcon;
                    }
                case WeaponType.Laser:
                    {
                        return this.LaserIcon;
                    }
                case WeaponType.Plazma:
                    {
                        return this.PlasmaIcon;
                    }
                case WeaponType.MagnetohydrodynamicGun:
                    {
                        return this.MagnetoIcon;
                    }
                case WeaponType.Missile:
                    {
                        return this.MissileIcon;
                    }
                case WeaponType.Torpedo:
                    {
                        return this.TorpedoIcon;
                    }
                case WeaponType.Cannon:
                default:
                    {
                        return this.CanonIcon;
                    }
            }
        }
        private Texture IconOf(Type type)
        {
            if (type == typeof(MissileTrapLauncher))
                return MissoleTrapSpellIcon;
            else if (type == typeof(Jammer))
                return JammerSpellIcon;
            else if (type == typeof(Transponder))
                return TransponderSpellIcon;
            else if (type == typeof(Warp))
                return WarpSpellIcon;
            else if (type == typeof(RadarBooster))
                return RadarBoosterSpellIcon;
            else if (type == typeof(ShieldStunner))
                return ShieldStunnerSpellIcon;
            else if (type == typeof(TrusterStunner))
                return TrusterStunnerSpellIcon;
            else if (type == typeof(EmergencySelfRapairing))
                return EmergencySelfRapairingSpellIcon;
            else if (type == typeof(EmergencyShieldRecharging))
                return EmergencyShieldRechargingSpellIcon;
            else if (type == typeof(AcceleratingCoils))
                return AcceleratingCoilsSpellIcon;
            else if (type == typeof(RechargeAcceleratorPassive))
                return RechargeAcceleratorPassiveSpellIcon;
            else if (type == typeof(ExtendedAmmoPack))
                return ExtendedAmmoPackSpellIcon;
            else if (type == typeof(TeamSpirit))
                return TeamSpiritSpellIcon;
            else if (type == typeof(ForcedTargetDesignator))
                return ForcedTargetDesignatorSpellIcon;
        //    else if (type == typeof(type))
        //        return SpellIcon;
            else return DefaultSpellIcon;
        }

        private void Update()
        {
            if (Global.selectedList.Count == 1)
            {
                if ((object)observable != Global.selectedList[0])
                    SetObservable(Global.selectedList[0]);
                if (observable.ManualControl)//observable status under manual control
                {
                    maincam.TargetFollow = observable.GetTransform();
                    maincam.mode = OrbitalCamMode.ThirthPerson;
                    mode = ObserverMode.Full;
                    if (observable.CurrentTarget == null)
                    {
                        if (previevIsOpen)
                            previevIsOpen = false;
                    }
                    else
                    {
                        if (!previevIsOpen)
                            previevIsOpen = true;
                    }
                }
                else
                {
                    FindObjectOfType<UnitSelectionComponent>().enabled = true;
                    FindObjectOfType<ShipManualController>().enabled = false;
                    maincam.TargetFollow = observable.GetTransform();
                    maincam.mode = OrbitalCamMode.Folloving;
                    mode = ObserverMode.Half;
                    if (!previevIsOpen)
                    {
                        previevIsOpen = true;
                    }
                }
                if (!statusIsOpen)
                {
                    statusIsOpen = true;
                }
            }
            else
            {
                if (observable != null)
                {
                    observable.ManualControl = false;
                    ButtonOn();
                }
                maincam.TargetFollow = null;
                observable = null;
                FindObjectOfType<ShipManualController>().enabled = false;
                maincam.mode = OrbitalCamMode.Free;
                FindObjectOfType<UnitSelectionComponent>().enabled = true;
                mode = ObserverMode.None;
                if (statusIsOpen)
                {
                    statusIsOpen = false;
                }
                if (previevIsOpen)
                {
                    previevIsOpen = false;
                }
            }
            if (statusIsOpen)
            {
                if (statusCount < MovDuratuon)
                {
                    statusCount += Time.deltaTime;
                    if (statusCount > MovDuratuon) statusCount = MovDuratuon;
                    float speedFactor = 1;// Mathf.Cos((MovDuratuon - statusCount) * Mathf.PI * 2 / MovDuratuon + Mathf.PI) + 1;
                    float statusSpeedMotion = status.GetComponent<RectTransform>().rect.height * hud.scale / MovDuratuon * speedFactor;
                    float weaponSpeedMotion = weaponPanel.GetComponent<RectTransform>().rect.width * hud.scale / MovDuratuon * speedFactor;
                    status.transform.Translate(0, statusSpeedMotion * Time.deltaTime, 0);
                    weaponPanel.transform.Translate(weaponSpeedMotion * Time.deltaTime, 0, 0);
                }
            }
            else
            {
                if (statusCount > 0)
                {
                    statusCount -= Time.deltaTime;
                    if (statusCount < 0) statusCount = 0;
                    float speedFactor = 1;// Mathf.Cos((MovDuratuon - statusCount) * Mathf.PI * 2 / MovDuratuon + Mathf.PI) + 1;
                    float statusSpeedMotion = status.GetComponent<RectTransform>().rect.height * hud.scale / MovDuratuon * speedFactor;
                    float weaponSpeedMotion = weaponPanel.GetComponent<RectTransform>().rect.width * hud.scale / MovDuratuon * speedFactor;
                    status.transform.Translate(0, -statusSpeedMotion * Time.deltaTime, 0);
                    weaponPanel.transform.Translate(-weaponSpeedMotion * Time.deltaTime, 0, 0);
                }
            }
            if (previevIsOpen)
            {
                if (previevCount < MovDuratuon)
                {
                    previevCount += Time.deltaTime;
                    if (previevCount > MovDuratuon) previevCount = MovDuratuon;
                    float speedFactor = 1;// Mathf.Cos((MovDuratuon - previevCount) * Mathf.PI * 2 / MovDuratuon + Mathf.PI) + 1;
                    float previevSpeedMotion = previev.GetComponent<RectTransform>().rect.height * hud.scale / MovDuratuon * speedFactor;
                    previev.transform.Translate(-previevSpeedMotion * Time.deltaTime, 0, 0);
                }
            }
            else
            {
                if (previevCount > 0)
                {
                    previevCount -= Time.deltaTime;
                    if (previevCount < 0) previevCount = 0;
                    float speedFactor = 1;// Mathf.Cos((MovDuratuon - previevCount) * Mathf.PI * 2 / MovDuratuon + Mathf.PI) + 1;
                    float previevSpeedMotion = previev.GetComponent<RectTransform>().rect.height * hud.scale / MovDuratuon * speedFactor;
                    previev.transform.Translate(previevSpeedMotion * Time.deltaTime, 0, 0);
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
                SwichHandControl();
        }
        private void OnGUI()
        {
            //GUI.skin = hud.Skin;
            //if (Global.StaticProportion && hud.scale != 1)
            //    GUI.matrix = Matrix4x4.Scale(Vector3.one * hud.scale);
            if (mode != ObserverMode.None)
            {
                //modules
                if (observable.Module != null && observable.Module.Length > 0)
                {
                    for (int i = 0; i < observable.Module.Length; i++)
                    {
                        float current;

                        switch (observable.Module[i].State)
                        {
                            case SpellModuleState.Active:
                                {
                                    current = observable.Module[i].BackCount / observable.Module[i].ActiveTime;
                                    moduleActive[i].fillAmount = current;
                                    moduleCooldown[i].fillAmount = 0;
                                    break;
                                }
                            case SpellModuleState.Cooldown:
                                {
                                    current = observable.Module[i].BackCount / observable.Module[i].CoolingTime;
                                    moduleActive[i].fillAmount = 0;
                                    moduleCooldown[i].fillAmount = current;
                                    break;
                                }
                            case SpellModuleState.Ready:
                                {
                                    current = 0f;
                                    moduleActive[i].fillAmount = 0;
                                    moduleCooldown[i].fillAmount = 0;
                                    break;
                                }
                        }
                    }
                }
                //health
                {
                    ShieldBar.fillAmount = observable.ShieldForce / observable.ShieldCampacity;
                    ShieldCount.text = Mathf.Round(observable.ShieldForce).ToString();
                    HealthBar.fillAmount = observable.Health / observable.MaxHealth;
                    HealthCount.text = Mathf.Round(observable.Health).ToString();
                    //Rect panelRect = UIUtil.GetRect(new Vector2(healthPanel.width, healthPanel.height), PositionAnchor.LeftDown, hud.mainRect.size, new Vector2(10, -10));
                    //GUI.BeginGroup(panelRect, healthPanel);
                    //{
                    //    Rect healthBackRect = UIUtil.GetRect(new Vector2(healthBarBack.width, healthBarBack.height), PositionAnchor.LeftUp, panelRect.size, new Vector2(4, 4));
                    //    GUI.BeginGroup(healthBackRect, healthBarBack);

                    //}
                    //GUI.EndGroup();
                }
                //weapon
                {
                    for (int i = 0; i < observable.PrimaryWeapon.Length; i++)
                    {
                        if (observable.PrimaryWeapon[i].GetType() == typeof(EnergyWeapon))
                        {
                            float fill= observable.PrimaryWeapon[i].ShootCounter / observable.PrimaryWeapon[i].MaxShootCounter;
                            PrimaryCooldown[i].fillAmount = fill;
                            PrimaryCounters[i].color = new Color(255 * fill, 0, 255 * (1 - fill));
                        }
                        else if (observable.PrimaryWeapon[i].GetType() == typeof(MagWeapon))
                        {
                            if (observable.PrimaryWeapon[i].BackCounter < (60f / observable.PrimaryWeapon[i].Firerate))
                                PrimaryCooldown[i].fillAmount = observable.PrimaryWeapon[i].BackCounter / (60f / observable.PrimaryWeapon[i].Firerate);
                            else
                                PrimaryCooldown[i].fillAmount = observable.PrimaryWeapon[i].BackCounter / observable.PrimaryWeapon[i].MaxShootCounter;
                        }
                        else
                        {
                                PrimaryCooldown[i].fillAmount = observable.PrimaryWeapon[i].BackCounter / observable.PrimaryWeapon[i].MaxShootCounter;
                        }
                        PrimaryCounters[i].text = Mathf.RoundToInt(observable.PrimaryWeapon[i].ShootCounter).ToString();
                    }
                    for (int i = 0; i < observable.SecondaryWeapon.Length; i++)
                    {
                        if (observable.SecondaryWeapon[i].GetType() == typeof(EnergyWeapon))
                        {
                            float fill = observable.SecondaryWeapon[i].ShootCounter / observable.SecondaryWeapon[i].MaxShootCounter;
                            SecondaryCooldown[i].fillAmount = fill;
                            SecondaryCounters[i].color = new Color(255 * fill, 0, 255 * (1 - fill));
                        }
                        else if (observable.SecondaryWeapon[i].GetType() == typeof(MagWeapon))
                        {
                            if (observable.SecondaryWeapon[i].BackCounter < (60f / observable.SecondaryWeapon[i].Firerate))
                                SecondaryCooldown[i].fillAmount = observable.SecondaryWeapon[i].BackCounter / (60f / observable.SecondaryWeapon[i].Firerate);
                            else
                                SecondaryCooldown[i].fillAmount = observable.SecondaryWeapon[i].BackCounter / observable.SecondaryWeapon[i].MaxShootCounter;
                        }
                        else
                        {
                            SecondaryCooldown[i].fillAmount = observable.SecondaryWeapon[i].BackCounter / observable.SecondaryWeapon[i].MaxShootCounter;
                        }
                        SecondaryCounters[i].text = Mathf.RoundToInt(observable.SecondaryWeapon[i].ShootCounter).ToString();
                    }
                }
            }
        }
        public void ActiveModule(int index)
        {
            if (observable != null && index < observable.Module.Length && observable.Module[index].State == SpellModuleState.Ready)
                observable.Module[index].EnableIfReady();
        }
        Texture2D ProgressUpdate(float progress, Texture2D tex)
        {
            Texture2D thisTex = new Texture2D(tex.width, tex.height);
            Vector2 centre = new Vector2(Mathf.Ceil(thisTex.width / 2f), Mathf.Ceil(thisTex.height / 2f)); //find the centre pixel
            for (int y = 0; y < thisTex.height; y++)
            {
                for (int x = 0; x < thisTex.width; x++)
                {
                    var angle = Mathf.Atan2(x - centre.x, y - centre.y) * Mathf.Rad2Deg; //find the angle between the centre and this pixel (between -180 and 180)
                    if (angle < 0)
                    {
                        angle += 360; //change angles to go from 0 to 360
                    }
                    var pixColor = tex.GetPixel(x, y);
                    if (angle <= progress * 360.0)
                    { //if the angle is less than the progress angle blend the overlay colour
                        pixColor = new Color(0, 0, 0, 0);
                        thisTex.SetPixel(x, y, pixColor);
                    }
                    else
                    {
                        thisTex.SetPixel(x, y, pixColor);
                    }
                }
            }
            thisTex.Apply(); //apply the cahnges we made to the texture
            return thisTex;
        }
        public void SwichHandControl()
        {
            if (mode == ObserverMode.Half)
            {
                if (observable != null)
                {
                    observable.ManualControl = true;
                    if (observable.Type == UnitClass.LR_Corvette || observable.Type == UnitClass.Guard_Corvette || observable.Type == UnitClass.Support_Corvette)
                        maincam.targetOffset = new Vector3(0, 10, -15);
                    else if (observable.Type == UnitClass.Figther || observable.Type == UnitClass.Command || observable.Type == UnitClass.Bomber)
                        maincam.targetOffset = new Vector3(0, 5, -10);
                    else maincam.targetOffset = new Vector3(0, 1, -5);
                    maincam.TargetFollow = observable.GetTransform();
                    FindObjectOfType<ShipManualController>().owner = observable.GetTransform().GetComponent<SpaceShip>();
                    FindObjectOfType<ShipManualController>().enabled = true;
                }
                FindObjectOfType<UnitSelectionComponent>().enabled = false;
                mode = ObserverMode.Full;
                ButtonOff();
            }
            else
            {
                if (observable != null)
                {
                    observable.ManualControl = false;
                }
                FindObjectOfType<UnitSelectionComponent>().enabled = true;
                mode = ObserverMode.Half;
                ButtonOn();
            }
        }
        private void ButtonOff()
        {
                var colors = GameObject.Find("HandControlButton").GetComponent<Button>().colors;
                colors.normalColor = new Color(255, 0, 0, 255);
                colors.highlightedColor = new Color(255, 0, 255, 255);
                GameObject.Find("HandControlButton").GetComponent<Button>().colors = colors;
                GameObject.Find("HandControlButton").transform.GetChild(0).GetComponent<Text>().text = "OFF";
                GameObject.Find("HandControlButton").transform.GetChild(0).GetComponent<Text>().color = new Color(255, 0, 0, 255);
        }
        private void ButtonOn()
        {
                var colors = GameObject.Find("HandControlButton").GetComponent<Button>().colors;
                colors.normalColor = new Color(0, 255, 0, 255);
                colors.highlightedColor = new Color(0, 255, 255, 255);
                GameObject.Find("HandControlButton").GetComponent<Button>().colors = colors;
                GameObject.Find("HandControlButton").transform.GetChild(0).GetComponent<Text>().text = "ON";
                GameObject.Find("HandControlButton").transform.GetChild(0).GetComponent<Text>().color = new Color(0, 255, 75, 255);
        }
    }
}
