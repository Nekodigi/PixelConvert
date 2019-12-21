package com.example.examplemod.items;

import com.example.examplemod.ExampleMod;

import cpw.mods.fml.common.FMLCommonHandler;
import cpw.mods.fml.common.eventhandler.SubscribeEvent;
import cpw.mods.fml.common.gameevent.InputEvent.KeyInputEvent;
import net.minecraft.client.Minecraft;
import net.minecraft.creativetab.CreativeTabs;
import net.minecraft.entity.player.EntityPlayer;
import net.minecraft.item.Item;
import net.minecraft.item.ItemStack;
import net.minecraft.world.World;

public class itemTest extends Item {

	EntityPlayer player;
	public itemTest() {
		this.setCreativeTab(CreativeTabs.tabTools);
		FMLCommonHandler.instance().bus().register(this);
	}

	@SubscribeEvent
    public void KeyHandlingEvent(KeyInputEvent event) {
        if(ExampleMod.sampleKey.isPressed() && player != null) {
        	double yaw = Math.toRadians(player.rotationYaw);
    		player = Minecraft.getMinecraft().thePlayer;
    		player.motionX -= Math.sin(yaw)*1;
    		player.motionZ += Math.cos(yaw)*1;
    		System.out.print(yaw);
        }
    }

	@Override
	public ItemStack onItemRightClick(ItemStack item, World world, EntityPlayer entityPlayer) {
		player = entityPlayer;
		double yaw = Math.toRadians(player.rotationYaw);
		player = Minecraft.getMinecraft().thePlayer;
		player.motionX -= Math.sin(yaw)*1;
		player.motionZ += Math.cos(yaw)*1;
		return item;
	}
}
