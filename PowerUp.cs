using System.Collections.Generic;

namespace SpaceWarProject
{
    public class PowerUp : GameObject
    {
        public enum PowerUpType
        {
            Health,
            Shield,
            DoubleDamage,
            Speed
        }

        public PowerUpType Type { get; private set; }
        public bool IsActive { get; set; } = true;
        private double duration = 10.0; // Güçlendirici süresi (saniye)
        private double activeTime = 0;
        private bool effectActive = false;
        private Spaceship? player = null;

        public PowerUp(double x, double y, PowerUpType type) 
            : base(x, y, 30, 30) // 30x30 piksel powerup boyutu
        {
            Type = type;
        }

        public void ApplyPowerUp(Spaceship spaceship)
        {
            if (!IsActive || effectActive) return;

            player = spaceship;

            switch (Type)
            {
                case PowerUpType.Health:
                    player.TakeDamage(-50); // Can yenileme (-hasar = iyileştirme)
                    break;
                case PowerUpType.DoubleDamage:
                    player.DamageMultiplier = 2.0;
                    effectActive = true;
                    break;
                case PowerUpType.Shield:
                    player.HasShield = true;
                    effectActive = true;
                    break;
                case PowerUpType.Speed:
                    player.SpeedMultiplier = 1.5;
                    effectActive = true;
                    break;
            }
            IsActive = false;
        }

        public void Update(double deltaTime)
        {
            if (!IsActive) return;

            // PowerUp'ı yavaşça aşağı hareket ettir
            spawnY += 1;

            // Ekrandan çıktıysa deaktif et
            if (spawnY > 600)
            {
                IsActive = false;
                return;
            }

            // Aktif efektlerin süresini kontrol et
            if (effectActive)
            {
                activeTime += deltaTime;
                if (activeTime >= duration)
                {
                    RemoveEffect();
                }
            }
        }

        private void RemoveEffect()
        {
            if (player == null) return;
            
            effectActive = false;
            activeTime = 0;
            
            // Efektleri kaldır
            switch (Type)
            {
                case PowerUpType.DoubleDamage:
                    player.DamageMultiplier = 1.0;
                    break;
                case PowerUpType.Shield:
                    player.HasShield = false;
                    break;
                case PowerUpType.Speed:
                    player.SpeedMultiplier = 1.0;
                    break;
            }
            
            player = null;
        }
    }
}