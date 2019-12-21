package com.example.examplemod.blocks;

import net.minecraft.tileentity.TileEntity;

public class TileEntityBlockTest extends TileEntity {
	public int frameSelect = 0;
	public boolean oldPowered;

	public void nextFrame() {
		frameSelect++;
		if(frameSelect>=blockTest.frame) {
			frameSelect = 0;
		}
	}
}
