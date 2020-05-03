using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using M;
namespace td
{
    public class MU_TD_GameManager : NetworkBehaviour
    {
        [SerializeField]
        GameObject bomb = null;
        #region values
        [SerializeField]
        Sprite[] numberSprites = null;
        [SerializeField]
        Image Number = null;
        int maxPoint = 3;
        List<GameObject> resettableObjects = new List<GameObject>();
        [SerializeField]
        GameObject Ymascotte = null, Bmascotte = null;
        [SerializeField]
        GameObject _camera = null, followCamera = null;
        public Td_Player playerA = new Td_Player();
        public Td_Player playerB = new Td_Player();
        string dance = "";
        Td_Player possessor;
        string winner = "";
        Transform current_BombSpawnPoint;
        Vector3 playerSpawnPos = Vector3.zero;
        #region returnValues

        public Td_Player returnPossessor()
        {
            return possessor;
        }
        public void GivePossessor(Team p)
        {
            if (p == playerA.t)
            {
                possessor = playerA;
            }
            else
            {
                possessor = playerB;
            }

        }

        #endregion
        [SerializeField]
        Text playerAtext = null;
        [SerializeField]
        Text playerBtext = null;
        bool canMakePoint = true;
        [SerializeField]
        Transform[] camPositions = null;
        [SerializeField]
        M_Td_Clock clock = null;
        SceneLoader sceneloader = null;
        #endregion

        #region resetting
        public IEnumerator resetStatus()
        {
            CmdResetStatus();
            yield return null;
        }
        void rst()
        {
            foreach (var item in resettableObjects)
            {
                if (!item.activeInHierarchy)
                {
                    item.gameObject.SetActive(true);
                }
                M_TD_Character c = item.GetComponent<M_TD_Character>();
                if (c != null)
                {
                    c.reset();
                }
            }
        }
        [ClientRpc]
        void RpcReset()
        {
            if (!isServer)
            {
                rst();
            }
        }
        [Command]
        void CmdResetStatus()
        {
            rst();
            RpcReset();
        }
        public void setResetableObjects(GameObject r)
        {
            resettableObjects.Add(r);
        }


        #endregion

        #region init
        public void StartGame()
        {
            sceneloader = GetComponentInChildren<SceneLoader>();
            M_TD_TeamDirector[] director = FindObjectsOfType<M_TD_TeamDirector>();
            foreach (var item in director)
            {
                item.Init(this);
            }
            playerAtext.text = playerA.point.ToString();
            playerBtext.text = playerB.point.ToString();
            playerA.bombSpawnpoint = Ymascotte.transform.GetChild(0).transform;
            playerB.bombSpawnpoint = Bmascotte.transform.GetChild(0).transform;

            StartCoroutine(countDownCo());

        }
        #endregion

        #region addpoint
        IEnumerator pointShow(Team t)
        {
            if (Soundmanager.instance != null)
                Soundmanager.instance.PlaySeByName("RefereeWhistle");
            yield return new WaitForSeconds(1);
            if (Soundmanager.instance != null)
                Soundmanager.instance.musicPause();
            followCamera.SetActive(false);
            _camera.SetActive(true);
            yield return new WaitForSeconds(1);
            if (t == playerA.t)
            {
                danceCheck(Ymascotte);
                _camera.transform.position = camPositions[0].position;
                current_BombSpawnPoint = playerA.bombSpawnpoint;

            }
            else
            {
                danceCheck(Bmascotte);
                _camera.transform.position = camPositions[1].position;

                current_BombSpawnPoint = playerB.bombSpawnpoint;
            }

            yield return new WaitForSeconds(5);
            CmdSpawnBomb();
            goTonextGame();
            yield return resetStatus();
            followCamera.SetActive(true);
            _camera.SetActive(false);
            canMakePoint = true;
            if (Soundmanager.instance != null)
                Soundmanager.instance.musicResume();
        }

        public void addPointToPlayer(Team tm)
        {
            CmdAddpoint(tm);
        }
        void addP(Team tm)
        {
            if (!canMakePoint) return;
            canMakePoint = false;
            int value = 0;
            if (tm == playerA.t)
            {
                playerA.point++;
                value = playerA.point;
            }
            else
            {
                playerB.point++;
                value = playerB.point;
            }
            if (value >= maxPoint)
            {
                if (playerA.point > playerB.point)
                {
                    winner = playerA.t.ToString();
                }
                else
                {
                    winner = playerB.t.ToString();
                }
                Win(tm);
                clock.gameObject.SetActive(false);
            }
            else
            {
                StartCoroutine(pointShow(tm));
            }
            playerAtext.text = playerA.point.ToString();
            playerBtext.text = playerB.point.ToString();
        }
        [Command]
        void CmdAddpoint(Team tm)
        {
            addP(tm);
            RpcAddPoint(tm);
        }
        [ClientRpc]
        void RpcAddPoint(Team t)
        {
            if (!isServer)
            {
                addP(t);
            }
        }
        #endregion

        void goTonextGame()
        {
            M_TD_PancakeBall.instance.resetStatus();
        }
        #region end
        private void Win(Team tm)
        {
            CmdEnd();
        }
        public IEnumerator gameEndCo()
        {
            if (playerA.point > playerB.point)
            {
                winner = "Team " + playerA.t + " Wins!";
            }
            else if (playerA.point == playerB.point)
            {
                winner = "Draw";
            }
            else
            {
                winner = "Team " + playerB.t + " Wins!";
            }
            if (Soundmanager.instance != null)
                Soundmanager.instance.PlaySeByName("RefereeWhistle");
            yield return new WaitForSeconds(1);
            if (Soundmanager.instance != null)
                Soundmanager.instance.PlaySeByName("WinSound");
            sceneloader.loadGAME();
            PlayerPrefs.SetString(StaticStrings.rugbyResult, winner);
        }
        [Command]
        void CmdEnd()
        {
            StartCoroutine(gameEndCo());
            RpcEnd();
        }
        [ClientRpc]
        void RpcEnd()
        {
            if (!isServer)
            {
                StartCoroutine(gameEndCo());
            }
        }
        #endregion
        void danceCheck(GameObject obj)
        {
            if (Soundmanager.instance != null)
                Soundmanager.instance.PlaySeByName("stadium");
            int rnd = UnityEngine.Random.Range(1, 5);
            switch (rnd)
            {
                case 1:
                    dance = "DANCE";
                    break;
                case 2:
                    dance = "SALSA1";
                    break;
                case 3:
                    dance = "SALSA2";
                    break;
                case 4:
                    dance = StaticStrings.samba;
                    break;
            }
            obj.GetComponent<Animator>().SetTrigger(dance);
        }

        IEnumerator countDownCo()
        {
            if (Soundmanager.instance != null)
                Soundmanager.instance.PlaySeByName(StaticStrings.CountDownSoundEffect);
            Number.enabled = true;
            Number.sprite = numberSprites[0];
            yield return new WaitForSeconds(1);
            Number.sprite = numberSprites[1];
            yield return new WaitForSeconds(1);
            Number.sprite = numberSprites[2];
            yield return new WaitForSeconds(1);
            Number.enabled = false;

            if (Soundmanager.instance != null)
                Soundmanager.instance.PlayBgmByName("TouchdownModeBGM");
            if (clock != null)
                clock.GaneStart = true;

        }

        void spawnBomb()
        {
            Instantiate(bomb, current_BombSpawnPoint.position, current_BombSpawnPoint.rotation);
        }
        [Command]
        void CmdSpawnBomb()
        {
            spawnBomb();
            RpcSpawnBomb();
        }

        [ClientRpc]
        void RpcSpawnBomb()
        {
            if (!isServer)
            {
                spawnBomb();
            }
        }

    }
}
    public class Td_Player
    {
        
         public int point;
         public Team t;
         public Transform door;
         public Transform bombSpawnpoint;

    }

