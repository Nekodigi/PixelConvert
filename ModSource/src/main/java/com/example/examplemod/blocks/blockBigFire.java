package com.example.examplemod.blocks;

import java.util.Random;

import net.minecraft.block.Block;
import net.minecraft.block.ITileEntityProvider;
import net.minecraft.block.material.Material;
import net.minecraft.client.Minecraft;
import net.minecraft.creativetab.CreativeTabs;
import net.minecraft.entity.item.EntityTNTPrimed;
import net.minecraft.entity.player.EntityPlayer;
import net.minecraft.tileentity.TileEntity;
import net.minecraft.world.World;

public class blockBigFire extends Block implements ITileEntityProvider {


	public blockBigFire() {
		super(Material.rock);
		this.setCreativeTab(CreativeTabs.tabBlock);
		this.setHardness(5.0F);
		this.setResistance(10.0F);
		this.setStepSound(Block.soundTypeMetal);
		this.setHarvestLevel("pickaxe", 2);
		this.setLightLevel(0.0F);
	}

	@Override
	public TileEntity createNewTileEntity(World world, int meta) {
		return new TileEntityBlockBigFire();
	}

	void fire(World world, int x, int y, int z, EntityPlayer player) {
		Random random = new Random();
		for(int i = 0; i < 300; i++) {
			float yaw = random.nextFloat() * (float)Math.PI * 2;
			float roll = random.nextFloat() * (float)Math.PI;
			float dist = random.nextFloat()*1;
			EntityTNTPrimed tntPrimed = new EntityTNTPrimed(world);
			tntPrimed.setPosition(x, y, z);
			tntPrimed.motionX = Math.cos(yaw)*dist;
			tntPrimed.motionY = Math.sin(yaw)*dist+1;
			tntPrimed.motionZ = Math.cos(roll)*dist;
			tntPrimed.fuse = 50;
			world.spawnEntityInWorld(tntPrimed);
		}
	}

	@Override
	public void onNeighborBlockChange(World world, int x, int y, int z, Block neightbourBlock) {
		boolean powered = world.isBlockIndirectlyGettingPowered(x, y, z);
		TileEntityBlockBigFire tileEntity = (TileEntityBlockBigFire)world.getTileEntity(x, y, z);
		if(powered && tileEntity.oldPowered == false) {
			fire(world, x, y, z, Minecraft.getMinecraft().thePlayer);
		}
		tileEntity.oldPowered = powered;
	}

	@Override
	public boolean onBlockActivated(World world, int x, int y, int z, EntityPlayer player, int side, float hitX, float hitY, float hitZ) {
		TileEntityBlockBigFire tileEntity = (TileEntityBlockBigFire)world.getTileEntity(x, y, z);
		tileEntity.world = world;
		tileEntity.x = x;
		tileEntity.y = y;
		tileEntity.z = z;
		tileEntity.player = player;
		tileEntity.blockBigFireV = this;
		tileEntity.fuse = 100f;
		System.out.println("FuseStart?");
		return true;
	}

}
