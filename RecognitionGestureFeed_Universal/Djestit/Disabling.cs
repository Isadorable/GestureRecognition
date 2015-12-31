﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace RecognitionGestureFeed_Universal.Djestit
{
    public class Disabling : Choice
    {
        /* Costruttori */
        public Disabling(Term term) : base(term)
        {
        }
        public Disabling(List<Term> terms) : base(terms)
        {
        }

        /*
        public override void feedToken(Token token)
        {
            if (this.state == expressionState.Complete || this.state == expressionState.Error)
                return;

            if (this.children != null && this.children.GetType() == typeof(List<Term>))
            {
                for (int index = 0; index < this.children.Count; index++)
                {
                    if (!this.children[index].excluded)
                    {
                        if (this.children[index].lookahead(token))
                        {
                            this.children[index].fire(token);
                        }
                        else
                        {
                            // Tollera un altro elemento

                            // the current sub-term is not able to handle the input
                            // sequence
                            this.children[index].excluded = true;
                            this.children[index].error(token);
                        }
                    }
                }
            }
        }
        /* Metodi */
        /*
        public override void fire(Token token)
        {
            this.feedToken(token);
            bool allExcluded = true;
            bool min = false;

            for(int index = 0; index < this.children.Count; index++)
            {
                if(!this.children[index].excluded)
                {
                    min = true;
                    allExcluded = false;
                    switch(this.children[index].state)
                    {
                        case expressionState.Complete:
                            if(index == this.children.Count - 1)
                            {
                                // the expression is completed when the
                                // last subterm is completed
                                this.complete(token);
                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    if(min)
                    {
                        // re-include terms with index > min for next 
                        // disabling term selection
                        this.children[index].excluded = false;
                        this.children[index].reset();
                    }
                }
            }
            if(allExcluded)
            {
                this.error(token);
            }
            //
            TokenFireArgs args = new TokenFireArgs(token, this);
            IsTokenFire(args);
        }*/

        /* Error Tolerance Manager */
        // Bisogna implementare la somiglianza!
        public override void fire(Token token)
        {
            this.feedToken(token);
            bool allExcluded = true;
            bool min = false;

            for (int index = 0; index < this.children.Count; index++)
            {
                if (!this.children[index].excluded)
                {
                    min = true;
                    allExcluded = false;
                    switch (this.children[index].state)
                    {
                        case expressionState.Complete:
                            if (index == this.children.Count - 1)
                            {
                                // the expression is completed when the
                                // last subterm is completed
                                this.complete(token);
                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    if (min)
                    {
                        // re-include terms with index > min for next 
                        // disabling term selection
                        this.children[index].excluded = false;
                        this.children[index].reset();
                    }
                }
            }
            if (allExcluded)
            {
                this.error(token);
            }
            //
            TokenFireArgs args = new TokenFireArgs(token, this);
            IsTokenFire(args);
        }
    }
}
