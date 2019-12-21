package com.example.examplemod.blocks;

import net.minecraft.entity.player.EntityPlayer;
import net.minecraft.tileentity.TileEntity;
import net.minecraft.world.World;

public class TileEntityBlockBigFire extends TileEntity {

	public boolean oldPowered;
	public Float fuse = Float.POSITIVE_INFINITY;
	public blockBigFire blockBigFireV;
	public World world;
	public int x;
	public int y;
	public int z;
	public EntityPlayer player;

	@Override
	public void updateEntity() {
		if(fuse-- < 0) {
			fuse = Float.POSITIVE_INFINITY;
			blockBigFireV.fire(world, x, y, z, player);
		}
	}
}
