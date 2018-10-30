using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CS4455.Utility {

    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyCenterOfMass : MonoBehaviour {


        [Tooltip("An optional marker GameObject used to identify where the center of mass is. If assigned non-null, it will be used to determine the centerOfMass")]
        public GameObject centerOfMassMarker = null;

        [Tooltip("The relative position defining the center of mass that will be used in physics simulation. Will be overridden by centerOfMassMarker if non-null.")]
        public Vector3 centerOfMass = Vector3.zero;

        [Tooltip("Whether the centerOfMass is re-evaluated every FixedUpdate()")]
        public bool continuousUpdating = false;

        protected Rigidbody rb;

        void Awake() {
            rb = GetComponent<Rigidbody>();
        }

    	
    	void Start () {

            AssignCenterOfMass();
            //centerOfMass = Vector3.up * -1;
           
    	}

        void FixedUpdate() {

            if (continuousUpdating)
                AssignCenterOfMass();
        }


        public void AssignCenterOfMass() {

            if (centerOfMassMarker != null)
            {
                centerOfMass = this.transform.InverseTransformPoint(centerOfMassMarker.transform.position);
            }
           
            if(rb.centerOfMass != centerOfMass)
                rb.centerOfMass = centerOfMass;
        }
    	

    }

}
