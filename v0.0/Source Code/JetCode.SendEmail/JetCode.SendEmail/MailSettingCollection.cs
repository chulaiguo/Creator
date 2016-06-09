using System;
using System.Collections;

namespace JetCode.SendEmail
{
    [Serializable]
    public class MailSettingCollection : CollectionBase
    {
        public void Add(MailSetting entity)
        {
            if (this.Contains(entity))
                return;

            base.List.Add(entity);
        }

        public void AddRange(MailSettingCollection list)
        {
            foreach (MailSetting item in list)
            {
                this.Add(item);
            }
        }

        public void Remove(MailSetting entity)
        {
            base.List.Remove(entity);
        }

        public void RemoveRange(MailSettingCollection list)
        {
            foreach (MailSetting item in list)
            {
                this.Remove(item);
            }
        }

        public void Insert(int index, MailSetting obj)
        {
            base.List.Insert(index, obj);
        }

        public MailSetting this[int index]
        {
            get
            {
                return (MailSetting)base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }

        public bool Contains(MailSetting item)
        {
            foreach (MailSetting data in base.List)
            {
                if (data.Equals(item))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
