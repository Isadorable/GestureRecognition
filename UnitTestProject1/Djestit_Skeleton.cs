﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Djestit
using RecognitionGestureFeed_Universal.Djestit;
// Gesture
using RecognitionGestureFeed_Universal.Gesture;
// Djestit Kinect
using RecognitionGestureFeed_Universal.Gesture.Kinect.Kinect_Djestit;
// JointInformation
using RecognitionGestureFeed_Universal.Recognition.Kinect.BodyStructure;
// Recognition
using RecognitionGestureFeed_Universal.Recognition;
// Feedback
using RecognitionGestureFeed_Universal.Feed.FeedBack;
// Feedback Handler/Modifies/Likelihood
using RecognitionGestureFeed_Universal.Feed.FeedBack.Tree.Wrapper.Handler;
using RecognitionGestureFeed_Universal.Feed.FeedBack.Tree.Wrapper.CustomAttributes;
using RecognitionGestureFeed_Universal.Feed.FeedBack.Tree.Wrapper.Likelihood;
// Kinect
using Microsoft.Kinect;
// Debug
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class Djestit_Skeleton
    {
        internal bool close(Token token)
        {
            if (token.GetType() == typeof(SkeletonToken))
            {
                //
                SkeletonToken skeletonToken = (SkeletonToken)token;
                if (skeletonToken.skeleton.rightHandStatus == HandState.Closed)
                {
                    return true;
                }
                else
                    return false;
            }
            return false;

        }
        internal bool moveX(Token token)
        {
            if (token.GetType() == typeof(SkeletonToken))
            {
                // 
                SkeletonToken skeletonToken = (SkeletonToken)token;

                if (skeletonToken.precSkeletons.Count > 1)
                {
                    //
                    float positionNewX = skeletonToken.positionX;
                    float positionNewY = skeletonToken.positionY;
                    //
                    List<float> listConfidenceX = new List<float>();
                    List<float> listConfidenceY = new List<float>();
                    // Calcolo la differenza lungo l'asse X e l'asse Y
                    foreach (Skeleton sOld in skeletonToken.precSkeletons)
                    {
                        // Preleva dal penultimo scheletro il JointInformation riguardante la mano
                        float positionOldX = sOld.handRightPositionX;
                        float positionOldY = sOld.handRightPositionY;
                        listConfidenceX.Add(Math.Abs(positionNewX - positionOldX));
                        listConfidenceY.Add(Math.Abs(positionNewY - positionOldY));
                    }
                    if (listConfidenceX.Average() > listConfidenceY.Average())
                        return true;
                    else
                        return false;
                }
                return true;
            }
            return false;
        }
        internal bool moveY(Token token)
        {
            if (token.GetType() == typeof(SkeletonToken))
            {
                SkeletonToken skeletonToken = (SkeletonToken)token;
                // Controlla se la mano destra è effettivamente chiusa e se c'è stato un qualche movimento (anche impercettibile)
                // Preleva dall'ultimo scheletro il JointInformation riguardante la mano
                float positionNewX = skeletonToken.positionX;
                float positionNewY = skeletonToken.positionY;
                //
                List<float> listConfidenceX = new List<float>();
                List<float> listConfidenceY = new List<float>();

                // Calcolo la differenza lungo l'asse X e l'asse Y
                foreach (Skeleton sOld in skeletonToken.precSkeletons)
                {
                    // Preleva dal penultimo scheletro il JointInformation riguardante la mano
                    float positionOldX = sOld.handRightPositionX;
                    float positionOldY = sOld.handRightPositionY;
                    listConfidenceX.Add(Math.Abs(positionNewX - positionOldX));
                    listConfidenceY.Add(Math.Abs(positionNewY - positionOldY));
                }
                if (listConfidenceX.Average() < listConfidenceY.Average())
                    return true;
                else
                    return false;
            }
            return false;
        }
        internal bool open(Token token)
        {
            if (token.GetType() == typeof(SkeletonToken))
            {
                SkeletonToken skeletonToken = (SkeletonToken)token;
                if (skeletonToken.skeleton.rightHandStatus == HandState.Open)
                    return true;
                else
                    return false;
            }
            return false;
        }

        [Modifies("a", 0), Modifies("b", 1), Modifies("c", 2)]
        void PanX(object sender, GestureEventArgs t)
        {
            Debug.WriteLine("Eseguito gesto PanX");
        }

        [Modifies("a",0), Modifies("d", 1), Modifies("e", 2)]
        void PanY(object sender, GestureEventArgs t)
        {
            Debug.WriteLine("Eseguito gesto PanY");
        }
        void Close(object sender, GestureEventArgs t)
        {
            Debug.WriteLine("Ho la mano destra chiusa.");
        }
        void MoveX(object sender, GestureEventArgs t)
        {
            Debug.WriteLine("Ho mosso la mano destra chiusa.");
        }
        void MoveY(object sender, GestureEventArgs t)
        {
            Debug.WriteLine("Ho mosso la mano destra chiusa.");
        }
        void Open(object sender, GestureEventArgs t)
        {
            Debug.WriteLine("Ho la mano destra aperta.");
        }

        [TestMethod]
        public void PanX()
        {

            // Sensore
            SkeletonSensor sensor;
            
            // Espressione
            // Close
            GroundTerm termx1 = new GroundTerm();
            termx1.type = "Start";
            termx1.accepts = close;
            termx1.name = "GroundTerm CloseX";
            termx1.likelihood = 0.5f;//new Likelihood(0.01f);
            termx1.Complete += Close;
            // Move
            GroundTerm termx2 = new GroundTerm();
            termx2.type = "Move";
            termx2.accepts = moveX;
            termx2.name = "GroundTerm MoveX";
            termx2.likelihood = 0.75f;//new Likelihood(0.02f);
            termx2.Complete += MoveX;
            // Open
            GroundTerm termx3 = new GroundTerm();
            termx3.type = "End";
            termx3.accepts = open;
            termx3.name = "GroundTerm OpenX";
            termx3.likelihood = 0.5f;//new Likelihood(0.01f);
            termx3.Complete += Open;
            Iterative iterativex = new Iterative(termx2);
            iterativex.likelihood = ComputeLikelihood.indipendentEvents(iterativex);//new Likelihood(iterativex, ProbabilityType.IndipendentEvents);
            List<Term> listTermx = new List<Term>();
            listTermx.Add(iterativex);
            listTermx.Add(termx3);
            Disabling disablingx = new Disabling(listTermx);
            disablingx.likelihood = ComputeLikelihood.indipendentEvents(disablingx); //new Likelihood(disablingx, ProbabilityType.IndipendentEvents);
            List<Term> listTerm2 = new List<Term>();
            listTerm2.Add(termx1);
            listTerm2.Add(disablingx);
            Sequence panX = new Sequence(listTerm2);
            panX.likelihood = ComputeLikelihood.indipendentEvents(panX); //new Likelihood(panX, ProbabilityType.IndipendentEvents);
            panX.Complete += PanX;
            panX.name = "PanX";
            // Handler PanX
            panX.handler = new Handler(this.PanX, panX, this.GetType().GetCustomAttributes(true).OfType<Modifies>().ToList());
            
            /* Pan Asse Y *
            // Close
            GroundTerm termy1 = new GroundTerm();
            termy1.type = "Start";
            termy1.accepts = close;
            termy1.name = "GroundTerm CloseY";
            termy1.likelihood = 0.5f;//new Likelihood(0.01f);
            termy1.Complete += Close;
            // Move
            GroundTerm termy2 = new GroundTerm();
            termy2.type = "Move";
            termy2.accepts = moveY;
            termy2.name = "GroundTerm MoveY";
            termy2.likelihood = 0.75f;// new Likelihood(0.3f);
            termy2.Complete += MoveY;
            // Open
            GroundTerm termy3 = new GroundTerm();
            termy3.type = "End";
            termy3.accepts = open;
            termy3.name = "GroundTerm OpenY";
            termy3.likelihood = 0.5f;// new Likelihood(0.01f);
            termy3.Complete += Open;
            Iterative iterativey = new Iterative(termy2);
            iterativey.likelihood = ComputeLikelihood.indipendentEvents(iterativey); //new Likelihood(iterativey, ProbabilityType.IndipendentEvents);
            List<Term> listTermy = new List<Term>();
            listTermy.Add(iterativey);
            listTermy.Add(termy3);
            Disabling disablingy = new Disabling(listTermy);
            disablingy.likelihood = ComputeLikelihood.indipendentEvents(disablingy); //new Likelihood(disablingy, ProbabilityType.IndipendentEvents);
            List<Term> listTermy2 = new List<Term>();
            listTermy2.Add(termy1);
            listTermy2.Add(disablingy);
            Sequence panY = new Sequence(listTermy2);
            panY.likelihood = ComputeLikelihood.indipendentEvents(panY); //new Likelihood(panY, ProbabilityType.IndipendentEvents);
            panY.Complete += PanY;
            panY.name = "PanY";
            // PanY
            panY.handler = new Handler(this.PanY, panY);*/

            // Choice
            List<Term> listTerm = new List<Term>();
            listTerm.Add(panX);
            //listTerm.Add(panY);
            Choice choice = new Choice(listTerm);
            // Assoccio l'espressione panX al sensor
            sensor = new SkeletonSensor(choice, 5);
            // Creo l'albero dei feedback
            //Feedback tree = new Feedback(choice);
           
            /// Simulazione Gesti
            // Simulo 1 gesto di start
            Skeleton sStart = new Skeleton(0, HandState.Closed, 0.0f, 0.0f);
            SkeletonToken tStart = (SkeletonToken)sensor.generateToken(TypeToken.Start, sStart);
            // E lo sparo al motorino
            sensor.root.fire(tStart);

            // Simulo 20 gesti di move
            for(int i = 0; i < 100; i++)
            {
                Skeleton sMove = null;
                SkeletonToken tMove = null;
                //sMove = new Skeleton(0, HandState.Closed, 0.0f+i, 0.0f);
                //tMove = (SkeletonToken)sensor.generateToken(TypeToken.Move, sMove);
                if(i == 20)
                {
                    sMove = new Skeleton(0, HandState.Closed, 0.0f, 10.0f);
                    tMove = (SkeletonToken)sensor.generateToken(TypeToken.Move, sMove);
                }
                else
                {
                    sMove = new Skeleton(0, HandState.Closed, 0.0f+i, 0.0f);
                    tMove = (SkeletonToken)sensor.generateToken(TypeToken.Move, sMove);
                }
                /*if (i == 1)
                {
                    sMove = new Skeleton(0, HandState.Closed, 0.1f, 0.0f);
                    tMove = (SkeletonToken)sensor.generateToken(TypeToken.Move, sMove);
                }
                else
                {
                    if (i == 140)
                        sMove = new Skeleton(0, HandState.Closed, (0.0f - 1000f), 0.0f);
                    if (i == 50)
                    {
                        i = 51;
                        sMove = new Skeleton(0, HandState.Closed, (1f + i), 0.0f);
                    }
                    // Creo lo scheletro
                    sMove = new Skeleton(0, HandState.Closed, (1f + i), 0.0f);
                    // Creo il gesto
                    tMove = (SkeletonToken)sensor.generateToken(TypeToken.Move, sMove);
                }*/
                // E lo sparo
                sensor.root.fire(tMove);
            }
            //tree.tree.print();

            // Simulo 1 gesto di end
            Skeleton sEnd = new Skeleton(0, HandState.Open, 22.0f, 0.0f);
            SkeletonToken tEnd = (SkeletonToken)sensor.generateToken(TypeToken.Move, sEnd);
            // E lo sparo al motorino
            sensor.root.fire(tEnd);
            //tree.tree.print();
        }
    }
}
