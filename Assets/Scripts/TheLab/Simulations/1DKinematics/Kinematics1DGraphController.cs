using UnityEngine;
using ChartAndGraph;

namespace Physiqia.TheLab
{
    public class Kinematics1DGraphController : MonoBehaviour
    {
        [Header("Consts")]
        public const byte ACC_TYPE_CONST = 1;
        public const byte ACC_TYPE_SIN = 2;
        public const byte ACC_TYPE_GROWING = 3;

        [Header("Graphs")]
        public GraphChartBase posGraph;
        public GraphChartBase velocityGraph;

        [Header("Values")]
        public float duration = 10f;
        public float acceleration;
        public float initialVelocity;
        public byte accType = Kinematics1DGraphController.ACC_TYPE_CONST;

        [Header("Categories")]
        private string accelerationCategory = "Acceleration";
        private string positionCategory = "Position";
        private string velocityCategory = "Velocity";

        /// <summary>
        /// 
        /// </summary>
        void Start() { }

        /// <summary>
        /// 
        /// </summary>
        void Update() { }

        /// <summary>
        /// 
        /// </summary> <summary>
        /// 
        /// </summary>
        public void RunSimulation()
        {
            GraphChartBase accelerationGraph = GetComponent<GraphChartBase>();

            if (accelerationGraph == null && posGraph && velocityGraph) return;

            if (accelerationGraph)
            {
                accelerationGraph.Scrollable = false;
                accelerationGraph.DataSource.StartBatch();
                accelerationGraph.DataSource.ClearCategory(accelerationCategory);
            }

            if (posGraph)
            {
                posGraph.Scrollable = false;
                posGraph.DataSource.StartBatch();
                posGraph.DataSource.ClearCategory(positionCategory);
            }

            if (velocityGraph)
            {
                velocityGraph.Scrollable = false;
                velocityGraph.DataSource.StartBatch();
                velocityGraph.DataSource.ClearCategory(velocityCategory);
            }

            float timeStep = 0.1f;
            float time = 0f;
            float velocity = initialVelocity;
            float position = 0f;

            while (time <= duration)
            {
                float accelValue = GetAccelerationValue(time);

                velocity += accelValue * timeStep;
                position += velocity * timeStep;

                if (accelerationGraph)
                    accelerationGraph.DataSource.AddPointToCategory(accelerationCategory, time, accelValue);

                if (posGraph)
                    posGraph.DataSource.AddPointToCategory(positionCategory, time, position);

                if (velocityGraph)
                    velocityGraph.DataSource.AddPointToCategory(velocityCategory, time, velocity);

                time += timeStep;
            }

            if (accelerationGraph)
                accelerationGraph.DataSource.EndBatch();

            if (posGraph)
                posGraph.DataSource.EndBatch();

            if (velocityGraph)
                velocityGraph.DataSource.EndBatch();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private float GetAccelerationValue(float time)
        {
            switch (accType)
            {
                case ACC_TYPE_SIN:
                    return Mathf.Sin(time) * acceleration;
                case ACC_TYPE_GROWING:
                    return acceleration * time;
                default:
                    return acceleration;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <param name="accelValue"></param>
        /// <returns></returns>
        private float CalculatePosition(float time, float accelValue)
        {
            return (initialVelocity * time) + (0.5f * accelValue * time * time);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetAcceleration(float value)
        {
            acceleration = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public void SetAccelerationType(byte type)
        {
            accType = type;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetInitialVelocity(float value)
        {
            initialVelocity = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetDuration(float value)
        {
            duration = value;
        }

        public void MultipleGraphMode(bool value)
        {
            // TODO: IMPLEMENTE 1/3 GRAPHS SWITCH
        }
    }
}

