using RWCustom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WalekksBasement
{
    public class ChargedLantern : Weapon
    {
        private static float Rand => Random.value;

        new public float rotation;
        new public float lastRotation;
        public float rotVel;
        public float lastDarkness = -1f;
        public float darkness;

        private LightSource lightSource = null;

        private Color blackColor;
        private Color earthColor;

        private readonly float rotationOffset;

        public ChargedLanternAbstract Abstr { get; }

        public ChargedLantern(ChargedLanternAbstract abstr, Vector2 pos, Vector2 vel) : base(abstr, abstr.world)
        {
            Abstr = abstr;

            bodyChunks = new[] { new BodyChunk(this, 0, pos + vel, 4 * (Abstr.scaleX + Abstr.scaleY), 0.35f) { goThroughFloors = true } };
            bodyChunks[0].lastPos = bodyChunks[0].pos;
            bodyChunks[0].vel = vel;

            bodyChunkConnections = new BodyChunkConnection[0];
            airFriction = 0.999f;
            gravity = 0.9f;
            bounce = 0.6f;
            surfaceFriction = 0.45f;
            collisionLayer = 1;
            waterFriction = 0.92f;
            buoyancy = 0.75f;

            rotation = Rand * 360f;
            lastRotation = rotation;

            rotationOffset = Rand * 30 - 15;

            ResetVel(vel.magnitude);
        }

        public override void Update(bool eu)
        {
            /*if (Abstr.damage >= 1 && Random.value < 0.015f)
            {
                Shatter();
                return;
            }*/

            ChangeCollisionLayer(grabbedBy.Count == 0 ? 2 : 1);
            firstChunk.collideWithTerrain = grabbedBy.Count == 0;
            firstChunk.collideWithSlopes = grabbedBy.Count == 0;

            base.Update(eu);

            var chunk = firstChunk;

            lastRotation = rotation;
            rotation += rotVel * Vector2.Distance(chunk.lastPos, chunk.pos);

            rotation %= 360;

            if (grabbedBy.Count == 0)
            {
                if (firstChunk.lastPos == firstChunk.pos)
                {
                    rotVel *= 0.9f;
                }
                else if (Mathf.Abs(rotVel) <= 0.01f)
                {
                    ResetVel((firstChunk.lastPos - firstChunk.pos).magnitude);
                }
            }
            else
            {
                var grabberChunk = grabbedBy[0].grabber.mainBodyChunk;
                rotVel *= 0.9f;
                rotation = Mathf.Lerp(rotation, grabberChunk.Rotation.GetAngle() + rotationOffset, 0.25f);
            }

            if (!Custom.DistLess(chunk.lastPos, chunk.pos, 3f) && room.GetTile(chunk.pos).Solid && !room.GetTile(chunk.lastPos).Solid)
            {
                var firstSolid = SharedPhysics.RayTraceTilesForTerrainReturnFirstSolid(room, room.GetTilePosition(chunk.lastPos), room.GetTilePosition(chunk.pos));
                if (firstSolid != null)
                {
                    FloatRect floatRect = Custom.RectCollision(chunk.pos, chunk.lastPos, room.TileRect(firstSolid.Value).Grow(2f));
                    chunk.pos = floatRect.GetCorner(FloatRect.CornerLabel.D);
                    bool flag = false;
                    if (floatRect.GetCorner(FloatRect.CornerLabel.B).x < 0f)
                    {
                        chunk.vel.x = Mathf.Abs(chunk.vel.x) * 0.15f;
                        flag = true;
                    }
                    else if (floatRect.GetCorner(FloatRect.CornerLabel.B).x > 0f)
                    {
                        chunk.vel.x = -Mathf.Abs(chunk.vel.x) * 0.15f;
                        flag = true;
                    }
                    else if (floatRect.GetCorner(FloatRect.CornerLabel.B).y < 0f)
                    {
                        chunk.vel.y = Mathf.Abs(chunk.vel.y) * 0.15f;
                        flag = true;
                    }
                    else if (floatRect.GetCorner(FloatRect.CornerLabel.B).y > 0f)
                    {
                        chunk.vel.y = -Mathf.Abs(chunk.vel.y) * 0.15f;
                        flag = true;
                    }
                    if (flag)
                    {
                        rotVel *= 0.8f;
                    }
                }
            }

            if (this.lightSource == null)
            {
                this.lightSource = new LightSource(base.firstChunk.pos, false, new Color(1f, 0f, 0f), this);
                this.lightSource.affectedByPaletteDarkness = 0.5f;
                this.room.AddObject(this.lightSource);
            }
            else
            {
                this.lightSource.setPos = new Vector2?(base.firstChunk.pos);
                this.lightSource.setRad = (300f * Abstr.fuel) * Random.Range(1, 1.2f);
                this.lightSource.setAlpha = new float?(1f);
                if (this.lightSource.slatedForDeletetion || this.lightSource.room != this.room)
                {
                    this.lightSource = null;
                }
            }

            if (Abstr.fuel < 0f)
            {
                Abstr.fuel = 5;
            }
            if (Abstr.fuel > 0.12f)
            {
                Abstr.fuel -= 0.001f;
            }
        }

        public override void TerrainImpact(int chunk, IntVector2 direction, float speed, bool firstContact)
        {
            base.TerrainImpact(chunk, direction, speed, firstContact);

            if (speed > 10)
            {
                room.PlaySound(SoundID.Spear_Fragment_Bounce, firstChunk.pos, 0.35f, 2f);
                ResetVel(speed);
            }
        }

        private void ResetVel(float speed)
        {
            rotVel = Mathf.Lerp(-1f, 1f, Rand) * Custom.LerpMap(speed, 0f, 18f, 5f, 26f);
        }

        public override void ChangeMode(Mode newMode)
        { }

        public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            sLeaser.sprites = new FSprite[2];
            sLeaser.sprites[0] = new("icon_ChargedLantern", true);
            sLeaser.sprites[1] = new("Futile_White", true);
            sLeaser.sprites[1].shader = rCam.game.rainWorld.Shaders["UnderWaterLight"];
            //sLeaser.sprites[1].shader = rCam.game.rainWorld.Shaders["FlatLightNoisy"];

            //sLeaser.sprites[1] = new FSprite("pixel", true);
            AddToContainer(sLeaser, rCam, null);
        }

        public override void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
        {
            Vector2 pos = Vector2.Lerp(firstChunk.lastPos, firstChunk.pos, timeStacker);
            float num = Mathf.InverseLerp(305f, 380f, timeStacker);
            pos.y -= 20f * Mathf.Pow(num, 3f);
            float num2 = Mathf.Pow(1f - num, 0.25f);
            lastDarkness = darkness;
            darkness = rCam.room.Darkness(pos);
            darkness *= 1f - 0.5f * rCam.room.LightSourceExposure(pos);

            for (int i = 0; i < sLeaser.sprites.Length; i++)
            {
                sLeaser.sprites[i].x = pos.x - camPos.x;
                sLeaser.sprites[i].y = pos.y - camPos.y;
                sLeaser.sprites[i].rotation = Mathf.Lerp(Mathf.Lerp(lastRotation, rotation, timeStacker) + 90, 0, 0.8f);
                sLeaser.sprites[i].scaleY = (num2 * Abstr.scaleY);
                sLeaser.sprites[i].scaleX = (num2 * Abstr.scaleX);
                sLeaser.sprites[i].scaleY *= 0.25f;
                sLeaser.sprites[i].scaleX *= 0.25f;
            }

            if (grabbedBy.Count == 1)
            {
                sLeaser.sprites[0].anchorY = 1.25f;
                sLeaser.sprites[1].anchorY = 0.65f;
            }
            else
            {
                sLeaser.sprites[0].anchorY = 0.5f;
                sLeaser.sprites[1].anchorY = 0.5f;
            }

            Debug.Log(grabbedBy.Count);
            
            sLeaser.sprites[1].scale *= 20f;
            sLeaser.sprites[1].color = new Color(1, 0, 0);

            sLeaser.sprites[0].color = blackColor;
            //sLeaser.sprites[0].scaleY *= 1.175f - Abstr.damage * 0.2f;
            //sLeaser.sprites[0].scaleX *= 1.175f - Abstr.damage * 0.2f;

            //sLeaser.sprites[1].shader = rCam.room.game.rainWorld.Shaders["EnergyCell"];

            if (blink > 0 && Rand < 0.5f)
            {
                sLeaser.sprites[0].color = blinkColor;
            }
            else if (num > 0.3f)
            {
                sLeaser.sprites[0].color = Color.Lerp(sLeaser.sprites[0].color, earthColor, Mathf.Pow(Mathf.InverseLerp(0.3f, 1f, num), 1.6f));
            }

            if (slatedForDeletetion || room != rCam.room)
            {
                sLeaser.CleanSpritesAndRemove();
            }
        }

        public override void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
        {
            blackColor = palette.blackColor;
            earthColor = Color.Lerp(palette.fogColor, palette.blackColor, 0.5f);
        }

        public override void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer? newContainer)
        {
            newContainer ??= rCam.ReturnFContainer("Items");

            foreach (FSprite fsprite in sLeaser.sprites)
            {
                fsprite.RemoveFromContainer();
                newContainer.AddChild(fsprite);
            }
        }
    }
}
