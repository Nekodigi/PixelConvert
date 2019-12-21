package com.example.examplemod.blocks;

import java.io.IOException;
import java.nio.charset.Charset;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.stream.Collectors;

import net.minecraft.block.Block;
import net.minecraft.block.ITileEntityProvider;
import net.minecraft.block.material.Material;
import net.minecraft.client.Minecraft;
import net.minecraft.creativetab.CreativeTabs;
import net.minecraft.entity.player.EntityPlayer;
import net.minecraft.tileentity.TileEntity;
import net.minecraft.world.World;

public class blockTest extends Block implements ITileEntityProvider {

	int[][] datas = {{0, 1, 2, 3}, {4, 5, 6, 7}, {8, 9, 10, 11}, {12, 13, 14, 15}};
	public static int width = 4;
	public static int height = 4;
	public static int frame = 1;
	public int frameSelect = 0;
	String path = "C:/Users/uekaz/Downloads/Example.txt";
	String rawReadData;
	String[] readData;

	public blockTest(Material material) {
		super(material);
		this.setCreativeTab(CreativeTabs.tabBlock);
		this.setHardness(5.0F);
		this.setResistance(10.0F);
		this.setStepSound(Block.soundTypeMetal);
		this.setHarvestLevel("pickaxe", 2);
		this.setLightLevel(0.0F);

	}

	@Override
	public TileEntity createNewTileEntity(World world, int meta) {
		return new TileEntityBlockTest();
	}

	@Override
	public void onNeighborBlockChange(World world, int x, int y, int z, Block neightbourBlock) {
		boolean powered = world.isBlockIndirectlyGettingPowered(x, y, z);
		TileEntityBlockTest tileEntity = (TileEntityBlockTest)world.getTileEntity(x, y, z);
		if(powered && tileEntity.oldPowered == false) {
			Create(world, x, y, z);
		}
		tileEntity.oldPowered = powered;
	}

	@Override
	public boolean onBlockActivated(World world, int x, int y, int z, EntityPlayer player, int side, float hitX, float hitY, float hitZ) {
		Create(world, x, y, z);
		return true;
	}

	public void Create(World world, int x, int y, int z) {
		try{
			String dataPath = Minecraft.getMinecraft().mcDataDir.getPath();
			rawReadData = readAll(dataPath+"/resource/Example.txt");
			readData = rawReadData.split(",",0);
			width = Integer.parseInt(readData[0]);
			height = Integer.parseInt(readData[1]);
			frame = Integer.parseInt(readData[2]);
			System.out.println("roadSuccess------------------------------------------------"+rawReadData);
			System.out.println(dataPath);

		}
		catch (IOException e){
			System.out.println("roadFailed--------------------------------------------------------");
		}
		finally {

		}
		TileEntityBlockTest tileEntity = (TileEntityBlockTest)world.getTileEntity(x, y, z);
		tileEntity.nextFrame();
		frameSelect++;
		if(frameSelect>=frame) {
			frameSelect = 0;
		}
		System.out.println(tileEntity.frameSelect);
		for(int sy = 0; sy < height; sy++) {
			for(int sx = 0; sx < width; sx++) {
				if(Integer.parseInt(readData[tileEntity.frameSelect*width*height+sy*width+sx+3]) == -1) {
					world.setBlock(x + sx,  y + 1 + sy,  z, getBlockById(0), 0, 2);
				}
				else {
					world.setBlock(x + sx,  y + 1 + sy,  z, getBlockById(35), Integer.parseInt(readData[tileEntity.frameSelect*width*height+sy*width+sx+2]), 2);
				}
			}
		}
	}

	public static String readAll(final String path) throws IOException {
		return Files.lines(Paths.get(path), Charset.forName("UTF-8"))
		        .collect(Collectors.joining(System.getProperty("line.separator")));
	}

}
