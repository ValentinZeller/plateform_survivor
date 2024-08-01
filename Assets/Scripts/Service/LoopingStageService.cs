using PlateformSurvivor.InteractiveObject;
using PlateformSurvivor.Service;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlateformSurvivor
{
    public class LoopingStageService : MonoBehaviour
    {
        [SerializeField] int stagePart;
        [SerializeField] Transform playerTransform;
        
        void Start()
        {
            EventManager.AddListener("move_stage", MoveStagePart);
        }

        void Update()
        {
        
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer != LayerMask.NameToLayer("Ground") && collision.gameObject.layer != LayerMask.NameToLayer("Spike"))
            {
                collision.transform.SetParent(transform,true);
               
            }

            if (collision.gameObject.CompareTag("Player"))
            {
                if (stagePart == 0)
                {
                   EventManager.Trigger("move_stage", 1);
                } else if (stagePart == 2)
                {
                   EventManager.Trigger("move_stage", -1);
                }
            }
        }

        private void MoveStagePart(object data)
        {
            int movePart = (int)data;

            stagePart = (stagePart + movePart) % 3;
            if (stagePart < 0)
            {
                stagePart = 2;
            }

            transform.SetPositionAndRotation(new Vector3((stagePart-1) * 60, transform.position.y, transform.position.z), transform.rotation);
        }
    }
}
