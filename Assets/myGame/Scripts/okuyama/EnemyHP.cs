using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perapera_Puroto
{
    public class EnemyHP : HPManager , IDamageApplicable ,Character
    {
        /// <summary>�G�l�~�[��HP</summary>
        [SerializeField] float _enemyHp;
        /// <summary>���Z����X�R�A</summary>
        int ADD_SCORE = 1;
        /// <summary>HP�̉���</summary>
        int  MINI_HP = 0;


        /// <summary>
        /// �C���^�[�t�F�[�XCharacter�̃v���p�e�B�̃I�[�o�[���C�h
        /// </summary>
        private CharacterParameter _parameter;
        public CharacterParameter characterParameter
        {
            get => _parameter;
            //set => _parameter = value;
        }


        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                _enemyHp -= IDamage._damage;
                
                if (_enemyHp <= MINI_HP)
                {
                    //gameObject.SetActive(false);
                    Destroy(gameObject);
                    ScoreManager.AddScore(ADD_SCORE);
                }

            }
        }
        public void Damage(int damage)
        {
            _enemyHp -= damage;
            if (_enemyHp <= MINI_HP)
            {
                gameObject.SetActive(false);
                ScoreManager.AddScore(ADD_SCORE);
            }
        }

        public void AddDamage(Damage damage)
        {
            //if ()
            //{
                _enemyHp -= DamageCalculator.GetDamage(damage);
            //}

            if (_enemyHp <= MINI_HP)
            {
                gameObject.SetActive(false);
                ScoreManager.AddScore(ADD_SCORE);
            }
        }
    }
}
