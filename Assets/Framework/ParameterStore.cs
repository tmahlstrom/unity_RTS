using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RTS{
    public class ParameterStore : MonoBehaviour {
        
        public static ParameterStore Instance;


        public Material allowedMaterial;
        public Material notAllowedMaterial;
        public Material invisibleMaterial;



        private void Awake() {
            Instance = this;
        }





    }
}
