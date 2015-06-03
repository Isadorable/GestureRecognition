﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Djestit
using RecognitionGestureFeed_Universal.Djestit;

namespace RecognitionGestureFeed_Universal.GestureManager.Gesture_Djestit
{
    class SkeletonStateSequence : StateSequence
    {
        /* Attributi */
        internal Dictionary<int, List<SkeletonToken>> moves;
        internal Dictionary<int, int> m_index;

        /* costruttore */
        public SkeletonStateSequence(int capacity)
            : base(capacity)
        {
            this.moves = new Dictionary<int, List<SkeletonToken>>();
            this.m_index = new Dictionary<int, int>();
        }

        /* Metodi */
        public void push(SkeletonToken token)
        {
            this._push(token);
            switch (token.type)
            {
                case TypeToken.Start:
                    this.moves.Add(token.identifier, new List<SkeletonToken>());
                    this.m_index.Add(token.identifier, 0);
                    goto case TypeToken.Move;
                case TypeToken.Move:
                    goto case TypeToken.End;
                case TypeToken.End:
                    List<SkeletonToken> t;
                    this.moves.TryGetValue(token.identifier, out t);

                    if (t.Count < this.capacity)//this.moves[(int)token.identifier].tokens.Count() < this.capacity)
                        t.Add(token);
                    //this.moves[(int)token.identifier].tokens.Add(token);
                    else
                    {
                        t.Insert(this.m_index[token.identifier], token);/******************************** Modificare */
                        //this.moves[(int)token.identifier].tokens.Insert(this.m_index[token.identifier], token);
                    }

                    int c;
                    this.m_index.TryGetValue(token.identifier, out c);
                    c = (c + 1) % this.capacity;
                    m_index[token.identifier] = c;
                    break;

            }
        }

        public SkeletonToken getById(int delay, int id)
        {
            int pos = 0;
            List<SkeletonToken> t;
            this.moves.TryGetValue(id, out t);
            int m_index_id;
            this.m_index.TryGetValue(id, out m_index_id);

            if (t.Count < this.capacity)
                pos = m_index_id - delay - 1;
            else
                pos = (m_index_id - delay - 1 + this.capacity) % this.capacity;

            return t[pos];
        }
    }
}
