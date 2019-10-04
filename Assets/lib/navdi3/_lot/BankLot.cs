namespace navdi3
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    public class BankLot : MonoBehaviour
    {
        public Dictionary<string, Bank> banks;
        public Bank this[string bankName]
        {
            get
            {
                if (banks == null) SetupBanks();
                if (!banks.ContainsKey(bankName)) banks.Add(bankName, transform.Find(bankName).GetComponent<Bank>());
                return banks[bankName];
            }
        }

        public bool ContainsBank(string bankName)
        {
            try
            {
                return (this[bankName] != null);
            } catch
            {
                return false;
            }
        }

        private void Start()
        {
            if (banks == null) SetupBanks();
        }

        void SetupBanks()
        {
            banks = new Dictionary<string, Bank>();
            List<GameObject> bankableObjects = new List<GameObject>();
            foreach(Transform child in transform)
            {
                var bank = child.gameObject.GetComponent<Bank>();
                if (bank) banks.Add(bank.name, bank);
                else bankableObjects.Add(child.gameObject);
            }
            foreach(var bob in bankableObjects)
            {
                var bank = new GameObject(bob.name).AddComponent<Bank>();
                bank._prefab = bob.gameObject;
                bank.transform.SetParent(this.transform);
                bob.transform.SetParent(bank.transform);
                bob.gameObject.SetActive(false);
            }
        }
    }
}
