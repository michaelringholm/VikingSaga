﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor
{
    public static class SampleXml
    {
        public static string Data =
@"<?xml version=""1.0"" encoding=""utf-16""?>
<LocationSerialization xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <LocationData>
    <MapLocationData>
      <Id>1</Id>
      <Location>
        <X>0.24623115577889448</X>
        <Y>0.53095975232198145</Y>
      </Location>
    </MapLocationData>
    <MapLocationData>
      <Id>2</Id>
      <Location>
        <X>0.25125628140703515</X>
        <Y>0.326625386996904</Y>
      </Location>
    </MapLocationData>
    <MapLocationData>
      <Id>3</Id>
      <Location>
        <X>0.044374009508716325</X>
        <Y>0.1875</Y>
      </Location>
    </MapLocationData>
    <MapLocationData>
      <Id>4</Id>
      <Location>
        <X>0.43898573692551507</X>
        <Y>0.4175</Y>
      </Location>
    </MapLocationData>
  </LocationData>
  <LocationLinks>
    <MapLocationLinkData>
      <Points>
        <Point>
          <X>0.24623115577889448</X>
          <Y>0.53095975232198145</Y>
        </Point>
        <Point>
          <X>0.25594294770206022</X>
          <Y>0.525</Y>
        </Point>
        <Point>
          <X>0.26465927099841524</X>
          <Y>0.5125</Y>
        </Point>
        <Point>
          <X>0.2900158478605388</X>
          <Y>0.48625</Y>
        </Point>
        <Point>
          <X>0.3011093502377179</X>
          <Y>0.465</Y>
        </Point>
        <Point>
          <X>0.32567353407290017</X>
          <Y>0.43875</Y>
        </Point>
        <Point>
          <X>0.34786053882725831</X>
          <Y>0.425</Y>
        </Point>
        <Point>
          <X>0.38351822503961963</X>
          <Y>0.42</Y>
        </Point>
        <Point>
          <X>0.41521394611727419</X>
          <Y>0.42125</Y>
        </Point>
        <Point>
          <X>0.43898573692551507</X>
          <Y>0.4175</Y>
        </Point>
      </Points>
      <Node1Id>1</Node1Id>
      <Node2Id>4</Node2Id>
    </MapLocationLinkData>
    <MapLocationLinkData>
      <Points>
        <Point>
          <X>0.25125628140703515</X>
          <Y>0.326625386996904</Y>
        </Point>
        <Point>
          <X>0.22583201267828842</X>
          <Y>0.3125</Y>
        </Point>
        <Point>
          <X>0.2179080824088748</X>
          <Y>0.31</Y>
        </Point>
        <Point>
          <X>0.19572107765451663</X>
          <Y>0.31</Y>
        </Point>
        <Point>
          <X>0.18858954041204437</X>
          <Y>0.3025</Y>
        </Point>
        <Point>
          <X>0.17670364500792393</X>
          <Y>0.2925</Y>
        </Point>
        <Point>
          <X>0.16798732171156894</X>
          <Y>0.28875</Y>
        </Point>
        <Point>
          <X>0.14896988906497624</X>
          <Y>0.2875</Y>
        </Point>
        <Point>
          <X>0.14342313787638669</X>
          <Y>0.29125</Y>
        </Point>
        <Point>
          <X>0.1259904912836767</X>
          <Y>0.3075</Y>
        </Point>
        <Point>
          <X>0.11885895404120443</X>
          <Y>0.315</Y>
        </Point>
        <Point>
          <X>0.11093502377179081</X>
          <Y>0.32</Y>
        </Point>
        <Point>
          <X>0.094294770206022185</X>
          <Y>0.32875</Y>
        </Point>
        <Point>
          <X>0.087163232963549928</X>
          <Y>0.32375</Y>
        </Point>
        <Point>
          <X>0.083993660855784469</X>
          <Y>0.31</Y>
        </Point>
        <Point>
          <X>0.082408874801901746</X>
          <Y>0.29875</Y>
        </Point>
        <Point>
          <X>0.080824088748019024</X>
          <Y>0.28875</Y>
        </Point>
        <Point>
          <X>0.07210776545166403</X>
          <Y>0.27375</Y>
        </Point>
        <Point>
          <X>0.065768621236133126</X>
          <Y>0.2625</Y>
        </Point>
        <Point>
          <X>0.057052297939778132</X>
          <Y>0.24625</Y>
        </Point>
        <Point>
          <X>0.048335974643423138</X>
          <Y>0.23</Y>
        </Point>
        <Point>
          <X>0.045166402535657686</X>
          <Y>0.21</Y>
        </Point>
        <Point>
          <X>0.045166402535657686</X>
          <Y>0.2</Y>
        </Point>
        <Point>
          <X>0.044374009508716325</X>
          <Y>0.1875</Y>
        </Point>
      </Points>
      <Node1Id>2</Node1Id>
      <Node2Id>3</Node2Id>
    </MapLocationLinkData>
    <MapLocationLinkData>
      <Points>
        <Point>
          <X>0.25125628140703515</X>
          <Y>0.326625386996904</Y>
        </Point>
        <Point>
          <X>0.25911251980982569</X>
          <Y>0.32625</Y>
        </Point>
        <Point>
          <X>0.26862123613312205</X>
          <Y>0.32</Y>
        </Point>
        <Point>
          <X>0.27496038034865294</X>
          <Y>0.30375</Y>
        </Point>
        <Point>
          <X>0.2797147385103011</X>
          <Y>0.28</Y>
        </Point>
        <Point>
          <X>0.286053882725832</X>
          <Y>0.24875</Y>
        </Point>
        <Point>
          <X>0.30507131537242471</X>
          <Y>0.23875</Y>
        </Point>
        <Point>
          <X>0.33042789223454833</X>
          <Y>0.23875</Y>
        </Point>
        <Point>
          <X>0.358161648177496</X>
          <Y>0.24625</Y>
        </Point>
        <Point>
          <X>0.3763866877971474</X>
          <Y>0.2575</Y>
        </Point>
        <Point>
          <X>0.410459587955626</X>
          <Y>0.275</Y>
        </Point>
        <Point>
          <X>0.436608557844691</X>
          <Y>0.30875</Y>
        </Point>
        <Point>
          <X>0.44215530903328049</X>
          <Y>0.325</Y>
        </Point>
        <Point>
          <X>0.44453248811410462</X>
          <Y>0.36625</Y>
        </Point>
        <Point>
          <X>0.43898573692551507</X>
          <Y>0.38375</Y>
        </Point>
        <Point>
          <X>0.43740095087163233</X>
          <Y>0.39625</Y>
        </Point>
        <Point>
          <X>0.43898573692551507</X>
          <Y>0.4175</Y>
        </Point>
      </Points>
      <Node1Id>2</Node1Id>
      <Node2Id>4</Node2Id>
    </MapLocationLinkData>
    <MapLocationLinkData>
      <Points>
        <Point>
          <X>0.24623115577889448</X>
          <Y>0.53095975232198145</Y>
        </Point>
        <Point>
          <X>0.24247226624405704</X>
          <Y>0.52</Y>
        </Point>
        <Point>
          <X>0.24167987321711568</X>
          <Y>0.50375</Y>
        </Point>
        <Point>
          <X>0.24247226624405704</X>
          <Y>0.4875</Y>
        </Point>
        <Point>
          <X>0.26069730586370837</X>
          <Y>0.46125</Y>
        </Point>
        <Point>
          <X>0.27337559429477021</X>
          <Y>0.4475</Y>
        </Point>
        <Point>
          <X>0.28129952456418383</X>
          <Y>0.415</Y>
        </Point>
        <Point>
          <X>0.27892234548335976</X>
          <Y>0.38375</Y>
        </Point>
        <Point>
          <X>0.27020602218700474</X>
          <Y>0.36125</Y>
        </Point>
        <Point>
          <X>0.2583201267828843</X>
          <Y>0.35</Y>
        </Point>
        <Point>
          <X>0.25356576862123614</X>
          <Y>0.33875</Y>
        </Point>
        <Point>
          <X>0.25125628140703515</X>
          <Y>0.326625386996904</Y>
        </Point>
      </Points>
      <Node1Id>1</Node1Id>
      <Node2Id>2</Node2Id>
    </MapLocationLinkData>
  </LocationLinks>
</LocationSerialization>";
    }
}
