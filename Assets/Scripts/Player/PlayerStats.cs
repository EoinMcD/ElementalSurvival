using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] float maxHealth;
    [SerializeField] float healthRegen;
    [SerializeField] float maxWaitTimeForHealthRegen;
    [SerializeField] Slider healthBar;
    float healthDelayTimer;
    float health;
    bool selfHealing;

    [Header("Stamina")]
    [SerializeField] float maxStamina;
    [SerializeField] float staminaRegen;
    [SerializeField] float maxWaitTimeForStaminaRegen;
    [SerializeField] Slider staminaBar;
    [SerializeField] float staminaDrain;
    [SerializeField] float staminaDrainTimer;
    bool sprinting;
    float stamina;

    void Start() {
        health=maxHealth;
        stamina = maxStamina;
        selfHealing=false;
    }

    private void Update() {
        healthBar.value = health;
        healthBar.maxValue = maxHealth;
        staminaBar.value=stamina;
        staminaBar.maxValue=maxStamina;
        
    }

    #region  Health
    public void setHealth(float health){
        this.health=health;
    }

    public void addHealth(float health) {
        this.health+=health;
    }

    public float getHealth() {
        return stamina;
    }

    IEnumerator CountDownTimer() {
        while (healthDelayTimer >0) {
            healthDelayTimer-=1;
            yield return new WaitForSeconds(1);
        }
        StartCoroutine("SelfHeal");
    }

    IEnumerator SelfHeal() {
        while (health <maxHealth) {
            addHealth(healthRegen);
            yield return new WaitForSeconds(maxWaitTimeForHealthRegen);
        }
    }

    public void Damage(float damage){
        health -=damage;
        healthDelayTimer=maxWaitTimeForHealthRegen;
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
    public void setStamina(float stamina){
        this.stamina=stamina;
    }

    public void addStamina(float stamina) {
        this.stamina+=stamina;
    }

    public float getStamina() {
        return stamina;
    }

    public void StartSprinting(){
        sprinting=true;
        StartCoroutine("Sprinting");
        StopCoroutine("StaminaRegen");
    }

    IEnumerator Sprinting(){
        stamina-=staminaDrain;
        yield return new WaitForSeconds(staminaDrainTimer);
        StartCoroutine("Sprinting");
    }

    IEnumerator StaminaRegen() {
        while (stamina <maxStamina) {
            addStamina(staminaRegen);
            yield return new WaitForSeconds(maxWaitTimeForStaminaRegen);
        }
    }

    public void StopSprinting(){
        sprinting=false;
        StopCoroutine("Sprinting");
        StartCoroutine("StaminaRegen");
    }
    #endregion
}
