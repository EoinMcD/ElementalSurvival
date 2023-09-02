using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] int maxHealth;
    [SerializeField] int healthRegen;
    [SerializeField] float maxWaitTimeForRegen;
    [SerializeField] Slider healthBar;
    float timer;
    int health;
    bool selfHealing;

    [Header("Stamina")]
    [SerializeField] int maxStamina;
    [SerializeField] float staminaRegen;
    int stamina;

    void Start() {
        health=maxHealth;
        stamina = maxStamina;
        selfHealing=false;
    }

    private void Update() {
        healthBar.value = health;
        healthBar.maxValue = maxHealth;
        if (Input.GetKeyDown(KeyCode.E)) {
            StartCoroutine("SelfHeal");
        }
        if (Input.GetKeyDown(KeyCode.F)) {
            Damage(20);
        }
    }

    #region  Health
    public void setHealth(int health){
        this.health=health;
    }

    public void addHealth(int health) {
        this.health+=health;
    }

    public int getHealth() {
        return stamina;
    }

    IEnumerator CountDownTimer() {
        while (timer >0) {
            timer-=1;
            yield return new WaitForSeconds(1);
        }
        StartCoroutine("SelfHeal");
    }

    IEnumerator SelfHeal() {
        while (health <maxHealth) {
            addHealth(healthRegen);
            yield return new WaitForSeconds(maxWaitTimeForRegen);
        }
    }

    public void Damage(int damage){
        health -=damage;
        timer=maxWaitTimeForRegen;
        StopCoroutine("CountDownTimer");
        StopCoroutine("SelfHeal");
        StartCoroutine("CountDownTimer");
        if(health>=0) {
            //Player has died 
            //TODO - Sent event to game manager
        }
    }
    #endregion

    #region  Stamina
    public void setStamina(int stamina){
        this.stamina=stamina;
    }

    public void addStamina(int stamina) {
        this.stamina+=stamina;
    }

    public int getStamina() {
        return stamina;
    }
    #endregion
}
