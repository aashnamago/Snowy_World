//
//  RBVolumeButtons.m
//  VolumeSnap
//
//  Created by Randall Brown on 11/17/11.
//  Copyright (c) 2011 __MyCompanyName__. All rights reserved.
//

#import "RBVolumeButtons.h"
#import <AudioToolbox/AudioToolbox.h>
#import <MediaPlayer/MediaPlayer.h>

@interface RBVolumeButtons()
-(void)initializeVolumeButtonStealer;
-(void)volumeDown;
-(void)volumeUp;
-(void)applicationCameBack;
-(void)applicationWentAway;
@end

@implementation RBVolumeButtons

@synthesize upBlock;
@synthesize downBlock;
@synthesize launchVolume;

void volumeListenerCallback (
                             void                      *inClientData,
                             AudioSessionPropertyID    inID,
                             UInt32                    inDataSize,
                             const void                *inData
                             );
void volumeListenerCallback (
                             void                      *inClientData,
                             AudioSessionPropertyID    inID,
                             UInt32                    inDataSize,
                             const void                *inData
                             ){
   const float *volumePointer = (float*)inData;
   float volume = *volumePointer;

   
   if( volume > [(RBVolumeButtons*)inClientData launchVolume] )
   {
      [(RBVolumeButtons*)inClientData volumeUp];
   }
   else if( volume < [(RBVolumeButtons*)inClientData launchVolume] )
   {
      [(RBVolumeButtons*)inClientData volumeDown];
   }

}

-(void)volumeDown
{
   AudioSessionRemovePropertyListenerWithUserData(kAudioSessionProperty_CurrentHardwareOutputVolume, volumeListenerCallback, self);
   
   //NATE EDIT - We always reset the volume to 0.5 because we are treating these as buttons
   [[MPMusicPlayerController applicationMusicPlayer] setVolume:launchVolume];
   
   [self performSelector:@selector(initializeVolumeButtonStealer) withObject:self afterDelay:0.1];
   
   
   if( self.downBlock )
   {
      self.downBlock();
   }
}

-(void)volumeUp
{
   AudioSessionRemovePropertyListenerWithUserData(kAudioSessionProperty_CurrentHardwareOutputVolume, volumeListenerCallback, self);
   
   //NATE EDIT - We always reset the volume to 0.5 because we are treating these as buttons
   [[MPMusicPlayerController applicationMusicPlayer] setVolume:launchVolume];
   
   [self performSelector:@selector(initializeVolumeButtonStealer) withObject:self afterDelay:0.1];
   

      if( self.upBlock )
      {
         self.upBlock();
      }

}

-(id)init
{
   self = [super init];
   if( self )
   {
      AudioSessionInitialize(NULL, NULL, NULL, NULL);
      AudioSessionSetActive(YES);
      
	  //NATE EDIT - Set launch volume to 0.5
	  launchVolume = [MPMusicPlayerController applicationMusicPlayer].volume;
	  
	  
	  //[[MPMusicPlayerController applicationMusicPlayer] setVolume:0.5];
	  
      //launchVolume = 0.5;
      hadToLowerVolume = launchVolume == 1.0;
      hadToRaiseVolume = launchVolume == 0.0;
      justEnteredForeground = NO;
      
	  if( hadToLowerVolume )
      {
         [[MPMusicPlayerController applicationMusicPlayer] setVolume:0.95];
         launchVolume = 0.95;
         
      }
      
      if( hadToRaiseVolume )
      {
         [[MPMusicPlayerController applicationMusicPlayer] setVolume:0.05];
         launchVolume = 0.05;
      }
	  
      CGRect frame = CGRectMake(0, -100, 10, 0);
      MPVolumeView *volumeView = [[[MPVolumeView alloc] initWithFrame:frame] autorelease];
      [volumeView sizeToFit];
      [[[[UIApplication sharedApplication] windows] objectAtIndex:0] addSubview:volumeView];
      
      [self initializeVolumeButtonStealer];
      
      
      [[NSNotificationCenter defaultCenter] addObserverForName:UIApplicationWillResignActiveNotification object:nil queue:[NSOperationQueue mainQueue] usingBlock:^(NSNotification* notification){
         [self applicationWentAway];
      }];
      
      
      [[NSNotificationCenter defaultCenter] addObserverForName:UIApplicationDidBecomeActiveNotification object:nil queue:[NSOperationQueue mainQueue] usingBlock:^(NSNotification *notification){
         if( ! justEnteredForeground )
         {
            [self applicationCameBack];
         }
         justEnteredForeground = NO;
      }];
      
      
      [[NSNotificationCenter defaultCenter] addObserverForName:UIApplicationWillEnterForegroundNotification object:nil queue:[NSOperationQueue mainQueue] usingBlock:^(NSNotification *notification){
         AudioSessionInitialize(NULL, NULL, NULL, NULL);
         AudioSessionSetActive(YES);
         justEnteredForeground = YES;
         [self applicationCameBack];
         
         
      }];
      
   }
   return self;
}

-(void)applicationCameBack
{
   [self initializeVolumeButtonStealer];
   launchVolume = [[MPMusicPlayerController applicationMusicPlayer] volume];
   hadToLowerVolume = launchVolume == 1.0;
   hadToRaiseVolume = launchVolume == 0.0;
   if( hadToLowerVolume )
   {
      [[MPMusicPlayerController applicationMusicPlayer] setVolume:0.95];
      launchVolume = 0.95;
   }
   
   if( hadToRaiseVolume )
   {
      [[MPMusicPlayerController applicationMusicPlayer] setVolume:0.1];
      launchVolume = 0.05;
   }
}

-(void)applicationWentAway
{
   AudioSessionRemovePropertyListenerWithUserData(kAudioSessionProperty_CurrentHardwareOutputVolume, volumeListenerCallback, self);
   
   if( hadToLowerVolume )
   {
      [[MPMusicPlayerController applicationMusicPlayer] setVolume:1.0];
   }
   
   if( hadToRaiseVolume )
   {
      [[MPMusicPlayerController applicationMusicPlayer] setVolume:0.0];
   }
}

-(void)dealloc
{
   if( hadToLowerVolume )
   {
      [[MPMusicPlayerController applicationMusicPlayer] setVolume:1.0];
   }
   
   if( hadToRaiseVolume )
   {
      [[MPMusicPlayerController applicationMusicPlayer] setVolume:0.0];
   }
   [super dealloc];
}

-(void)initializeVolumeButtonStealer
{
   AudioSessionAddPropertyListener(kAudioSessionProperty_CurrentHardwareOutputVolume, volumeListenerCallback, self);
}

@end

//static NetServiceBrowserDelegate* delegateObject = nil;
//static NSNetServiceBrowser *serviceBrowser = nil;

static RBVolumeButtons *rbVolumeButtons = nil;
static int lastVolumeUpDown = 0;
//static int counter = 0;

// When native code plugin is implemented in .mm / .cpp file, then functions
// should be surrounded with extern "C" block to conform C function naming rules
extern "C" {

  void _Init()
  {
    //NSLog(@"INIT");
    rbVolumeButtons = [[[RBVolumeButtons alloc] init] autorelease];
    rbVolumeButtons.upBlock = ^{ 
      lastVolumeUpDown = 2;
    };
    rbVolumeButtons.downBlock = ^{ 
      lastVolumeUpDown = 1;
    };
	//NSLog(@"SUCCESS");
  }

  int _GetLastVolumePress()
  {
    //NSLog(@"VOLUME PRESS: %i", lastVolumeUpDown);
    int volumeUpDown = lastVolumeUpDown;
    lastVolumeUpDown = 0;
    return volumeUpDown;
  }
	
}

