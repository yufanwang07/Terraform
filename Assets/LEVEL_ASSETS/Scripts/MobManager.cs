using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Search;
using UnityEditor;
using Unity.Collections;


public class MobManager : MonoBehaviour
{

    public GameObject lineFX;
    public GameObject platformPrefab;
    public GameObject wallPrefab;
    public GameObject aura;
    private Outline threadShell;

    [Header("Audio Clips (Attacking)")]
    public AudioSource laserSingle;
    public AudioSource laserLoop;
    public AudioSource laserCharge;
    public AudioSource laserBig;

    [Header("Attack Prefabs")] 
    public GameObject atk1Prefab;
    public GameObject atk2Prefab;
    public GameObject atk3Prefab;
    public GameObject atk4Prefab;
    public GameObject cagePrefab;

    [Header("Misc References")] 
    public Interpreter gameLogic;
    public RespawnManager respawn;
    public Parallax cameraParallax;
    public GameObject averager;
    public Pulse pulser;
    public AudioSource bgm;
    public AudioSource errorSFX;
    public MoveToAdmin portalScript;
    


    private Rigidbody2D rb;
    [SerializeField] private Text commandLine;
    private GameObject spriteObject;
    public GameObject parent;

    [SerializeField] private GameObject _player;


    IEnumerator func() {
        GameObject target = GameObject.Find("Player");
        float r = Random.Range(1f, 1.5f); // Random radius between 1 and 2
        float theta = Random.Range(0f, Mathf.PI * 2); // Random angle between 0 and 2π
        laserSingle.Play();
        float x = r * Mathf.Cos(theta); // X component
        float y = r * Mathf.Sin(theta); // Y component
        GameObject attack = Instantiate(atk1Prefab, transform.position + new Vector3(x, y, 0), Quaternion.Euler(0, 0, 0));
        attack.name = "%";
        yield return null;
        Debug.Log("Attacking");
        StartCoroutine(Type1(target, attack));
    }


    void Start() {
        rb = GetComponent<Rigidbody2D>();
        spriteObject = GameObject.Find("MobSprite");
        StartCoroutine(spawnBulletSequence());
    }

    IEnumerator spawnBulletSequence(){
        while (true) {
            yield return new WaitForSeconds(1f);
            StartCoroutine(func());
        }
    }


    IEnumerator Type1(GameObject target, GameObject attack) {
        Vector3 opos = attack.transform.position;
        Vector3 tpos = target.transform.position;
        Vector3 modifier = new(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
        attack.transform.up = target.transform.position + modifier - attack.transform.position;


        yield return new WaitForSeconds(0.25f);
        for (int i = 1; i <= 1000; i++) {
            if (attack.IsDestroyed()) {
                break;
            }
            if (Mathf.Pow(attack.transform.position.x - target.transform.position.x, 2) + Mathf.Pow(attack.transform.position.y - target.transform.position.y, 2) <= 0.65f) {
                // break;
                _player.transform.position += Vector3.MoveTowards(attack.transform.position, attack.transform.position + target.transform.position + modifier - opos, Mathf.Min(5 * i, 300) / 900f) - attack.transform.position;
            }

            attack.transform.position = Vector3.MoveTowards(attack.transform.position, attack.transform.position + (tpos + modifier - opos) + (target.transform.position + modifier - opos), Mathf.Min(5 * i, 300) / 900f);
            yield return null;
        }
        if (!attack.IsDestroyed()) {
            Destroy(attack);
        }
    }
}

