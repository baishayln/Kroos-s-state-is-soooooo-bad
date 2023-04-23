using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerSkill
{
    void GetSkill();
    // Sprite GetSkillImage();
    float ReleaseSkill(Vector2 mousePosition);
}

public class MagellansFishBullet : PlayerSkill
{
    [SerializeField]public Sprite skillImage;
    [SerializeField]public AudioClip[] Voices;
    [SerializeField]public AudioClip FireSound;
    private float skillColdTime = 10;
    private PlayerShoot playerShoot;
    private PlayerMove playerMove;
    private Vector2 SkillReleasePoint;
    private CurveSingleSteeringLauncher launcher;
    public MagellansFishBullet(GameObject Player)
    {
        playerShoot = Player.GetComponent<PlayerShoot>();
        playerMove = Player.GetComponent<PlayerMove>();
        launcher = Player.transform.GetChild(2).GetComponent<CurveSingleSteeringLauncher>();
        Voices = playerShoot.MagellanVoices;
        FireSound = playerShoot.fishRocketFireSound;
    }
    public void GetSkill()
    {
        
    }
    
    public float ReleaseSkill(Vector2 mousePosition)
    {
        launcher.LaunchRocket(8 , 10);
        PlayVoice();
        PlayEffect();
        return skillColdTime;
    }
    public float ReleaseSkill(Vector2 mousePosition , float damage)
    {   
        //使用星星弹以玩家为中心点直接在其周围直接产生弹幕
        launcher.LaunchRocket(8 , damage);
        return skillColdTime;
    }
    private void PlayVoice()
    {
        SoundManager.Instance.PlayVoiceSound(Voices[Random.Range(0 , Voices.Length)]);
    }
    private void PlayEffect()
    {
        SoundManager.Instance.PlayEffectSound(FireSound);
    }
    // public Sprite GetSkillImage()
    // {
    //     return skillImage;
    // }
}

public class Kaminobazu : PlayerSkill
{
    // [SerializeField]public Sprite skillImage;
    private float skillColdTime = 12;
    private PlayerShoot playerShoot;
    private PlayerMove playerMove;
    private GameObject KaminobazuPrefab;
    [SerializeField]public AudioClip[] Voices;
    public Kaminobazu(GameObject Player)
    {
        playerShoot = Player.GetComponent<PlayerShoot>();
        playerMove = Player.GetComponent<PlayerMove>();
        KaminobazuPrefab = playerShoot.Kaminobazu;
        Voices = playerShoot.KamiVoices;
    }
    public void GetSkill()
    {
        
    }
    public float ReleaseSkill(Vector2 mousePosition)
    {
        GameObject shinbazu = ObjectPool.Instance.GetObject(KaminobazuPrefab);
        shinbazu.GetComponent<LightingArea>().SetData(4 , 15 , 0.5f , mousePosition);
        PlayVoice();
        return skillColdTime;
    }
    private void PlayVoice()
    {
        SoundManager.Instance.PlayVoiceSound(Voices[Random.Range(0 , Voices.Length)]);
    }
    // public Sprite GetSkillImage()
    // {
    //     return skillImage;
    // }
    void OnTriggerEnter2D()
    {

    }
}

public class GavialsAssistance : PlayerSkill        //从屏幕外上方一定区域内飞来一个嘉维尔的医疗杖，在飞过玩家头时触发一个表示敲击的动画，弹出“绑”字（嘉维尔为战场上奋战的克洛丝送来了援助，快说谢谢嘉维尔），命中后会飞到屏幕对侧的高处，做一个类似滑行降落的弹道效果，满血则会砸到地面上造成AOE，技能CD长
{
    // [SerializeField]public Sprite skillImage;
    private float skillColdTime = 16;
    private PlayerShoot playerShoot;
    private PlayerMove playerMove;
    private GameObject realMedicalSuppliesPrefab;
    [SerializeField]public AudioClip[] Voices;
    public GavialsAssistance(GameObject Player)
    {
        playerShoot = Player.GetComponent<PlayerShoot>();
        playerMove = Player.GetComponent<PlayerMove>();
        realMedicalSuppliesPrefab = playerShoot.realMedicalSupplies;
        Voices = playerShoot.GavialVoices;
    }
    public void GetSkill()
    {
        
    }
    // public Sprite GetSkillImage()
    // {
    //     return skillImage;
    // }
    public float ReleaseSkill(Vector2 mousePosition)
    {
        //指定一个X轴并用射线检查下方地面？如果没有地面则会射向下方一定距离的点？还是产生一个范围检测敌人进行辅助半自动瞄准？
        //不满血时无论如何都是直接奶人，满血时就算点自己也会直接进行攻击，攻击会产生大片石头？还是生成一些大小不同的石头四散坠落？例子特效？
        GameObject realMedicalSupplies = ObjectPool.Instance.GetObject(realMedicalSuppliesPrefab);
        realMedicalSupplies.GetComponent<GavialsSupport>().SetData(CameraBehaviour.Instance.ReturnBornPosition() , CameraBehaviour.Instance.ReturnCameraPosition() , playerMove.isFullOfHealth() , mousePosition , playerShoot.gameObject);
        PlayVoice();
        return skillColdTime;
    }
    private void PlayVoice()
    {
        
        SoundManager.Instance.PlayVoiceSound(Voices[Random.Range(0 , Voices.Length)]);
    }
}
