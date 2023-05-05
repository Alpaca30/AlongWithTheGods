using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public void Attack()
    {
        int random = Random.Range(0, 5);
        switch (random)
        {
            case 0:
                SoundManager.Instance.Play("AttackBody1");
                break;
            case 1:
                SoundManager.Instance.Play("AttackBody2");
                break;
            case 2:
                SoundManager.Instance.Play("AttackBody3");
                break;
            case 3:
                SoundManager.Instance.Play("AttackBody4");
                break;
            case 4:
                SoundManager.Instance.Play("AttackBody5");
                break;
        }
    }

    public void Hit()
    {
        int random = Random.Range(0, 4);
        switch (random)
        {
            case 0:
                SoundManager.Instance.Play("PlayerHit1");
                break;
            case 1:
                SoundManager.Instance.Play("PlayerHit2");
                break;
            case 2:
                SoundManager.Instance.Play("PlayerHit3");
                break;
            case 3:
                SoundManager.Instance.Play("PlayerHit4");
                break;
        }
    }

    public void Dead()
    {
        SoundManager.Instance.Play("PlayerDied");
    }

    public void Dash()
    {
        SoundManager.Instance.Play("Dash2");        
    }

    public void Walk()
    {
        int random = Random.Range(0, 2);
        switch (random)
        {
            case 0:
                SoundManager.Instance.Play("Walk1");
                break;
            case 1:
                SoundManager.Instance.Play("Walk2");
                break;
        }
    }

    public void SkillA1()
    {
        SoundManager.Instance.Play("SkillA1");
    }

    public void SkillA2()
    {
        SoundManager.Instance.Play("SkillA2");
    }

    public void SkillA3()
    {
        SoundManager.Instance.Play("SkillA3");
    }

    public void SkillA4()
    {
        SoundManager.Instance.Play("SkillA4");
    }

    public void SkillB1()
    {
        SoundManager.Instance.Play("SkillB1");
    }

    public void SkillB2()
    {
        SoundManager.Instance.Play("SkillB2");
    }

    public void SkillB3()
    {
        SoundManager.Instance.Play("SkillB3");
    }

    public void SkillB4()
    {
        SoundManager.Instance.Play("SkillB4");
    }
}
