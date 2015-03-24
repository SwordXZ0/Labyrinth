using UnityEngine;
using System.Collections;

public abstract class Filter{
	public string message;
	abstract public bool validate();
}
