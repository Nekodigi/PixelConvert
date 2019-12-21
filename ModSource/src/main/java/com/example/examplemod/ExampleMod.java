package com.example.examplemod;

import org.lwjgl.input.Keyboard;

import com.example.examplemod.blocks.blockBigFire;
import com.example.examplemod.blocks.blockTest;
import com.example.examplemod.items.itemTest;

import cpw.mods.fml.client.registry.ClientRegistry;
import cpw.mods.fml.common.Mod;
import cpw.mods.fml.common.Mod.EventHandler;
import cpw.mods.fml.common.event.FMLInitializationEvent;
import cpw.mods.fml.common.event.FMLPreInitializationEvent;
import cpw.mods.fml.common.registry.GameRegistry;
import net.minecraft.block.Block;
import net.minecraft.block.material.Material;
import net.minecraft.client.settings.KeyBinding;
import net.minecraft.init.Blocks;
import net.minecraft.item.Item;

@Mod(modid = ExampleMod.MODID, version = ExampleMod.VERSION)
public class ExampleMod
{
    public static final String MODID = "examplemod";
    public static final String VERSION = "1.0";

    public static Block testBlock;
    public static Block bigFireBlock;
    public static Item testItem;
    public static final KeyBinding sampleKey = new KeyBinding("Key.sample", Keyboard.KEY_R, "CategoryName");

    @EventHandler
    public void perInit(FMLPreInitializationEvent event) {
    	testBlock = new blockTest(Material.rock)
    	.setBlockName("testBlock")
    	.setBlockTextureName("");
    	GameRegistry.registerBlock(testBlock, "testBlock");
    	testItem = new itemTest()
    	.setUnlocalizedName("testItem")
    	.setTextureName("");
    	GameRegistry.registerItem(testItem, "testItem");
    	ClientRegistry.registerKeyBinding(sampleKey);
    	bigFireBlock = new blockBigFire()
    	.setBlockName("bigFireBlock")
    	.setBlockTextureName("bigFireBlock");
    	GameRegistry.registerBlock(bigFireBlock, "bigFireBlock");
    }

    @EventHandler
    public void init(FMLInitializationEvent event)
    {
		// some example code
        System.out.println("DIRT BLOCK >> "+Blocks.dirt.getUnlocalizedName());
    }
}
