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
        public GraphChartBase mainGraph;
        public GraphChartBase accelerationGraph;
        public GraphChartBase posGraph;
        public GraphChartBase velocityGraph;

        [Header("Values")]
        public float duration = 10f;
        public float acceleration;
        public float initialVelocity;
        public byte accType = Kinematics1DGraphController.ACC_TYPE_CONST;

        [Header("Mode")]
        public bool isTripleGraphMode = true;

        [Header("Categories")]
        private string zeroLineCategory = "ZeroLine";
        private string accelerationCategory = "Acceleration";
        private string positionCategory = "Position";
        private string velocityCategory = "Velocity";

        /// <summary>
        /// 
        /// </summary>
        void Start()
        {
            accelerationGraph = GetComponent<GraphChartBase>();
        }

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
            InitGraphs();

            float timeStep = 0.1f;
            float time = 0f;
            float velocity = initialVelocity;
            float position = 0f;

            while (time <= duration)
            {
                float accelValue = GetAccelerationValue(time);

                velocity += accelValue * timeStep;
                position += velocity * timeStep;

                if (isTripleGraphMode)
                {
                    if (accelerationGraph)
                        accelerationGraph.DataSource.AddPointToCategory(accelerationCategory, time, accelValue);

                    if (posGraph)
                        posGraph.DataSource.AddPointToCategory(positionCategory, time, position);

                    if (velocityGraph)
                        velocityGraph.DataSource.AddPointToCategory(velocityCategory, time, velocity);
                }
                else
                {
                    mainGraph.DataSource.AddPointToCategory(accelerationCategory, time, accelValue);
                    mainGraph.DataSource.AddPointToCategory(positionCategory, time, position);
                    mainGraph.DataSource.AddPointToCategory(velocityCategory, time, velocity);
                }

                time += timeStep;
            }

            EndBatchGraphs();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetGraphMode(bool value)
        {
            isTripleGraphMode = value;
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitGraphs()
        {
            if (!isTripleGraphMode)
            {
                mainGraph.Scrollable = false;
                mainGraph.DataSource.StartBatch();
                mainGraph.DataSource.ClearCategory(accelerationCategory);
                mainGraph.DataSource.ClearCategory(positionCategory);
                mainGraph.DataSource.ClearCategory(velocityCategory);

                return;
            }

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
        }

        /// <summary>
        /// 
        /// </summary>
        private void EndBatchGraphs()
        {
            if (!isTripleGraphMode)
            {
                mainGraph.DataSource.EndBatch();
                return;
            }

            if (accelerationGraph)
                accelerationGraph.DataSource.EndBatch();

            if (posGraph)
                posGraph.DataSource.EndBatch();

            if (velocityGraph)
                velocityGraph.DataSource.EndBatch();
        }
    }
}
